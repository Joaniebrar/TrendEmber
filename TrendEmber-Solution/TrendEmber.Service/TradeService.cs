using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Net.Http;
using System.Text.Json;
using System.Text.RegularExpressions;
using TrendEmber.Core.Trends;
using TrendEmber.Data;

namespace TrendEmber.Service
{
    public class TradeService : ITradeService
    {
        private TrendsDbContext _dbContext;
        public TradeService(TrendsDbContext trendsDbContext) { 
            _dbContext = trendsDbContext;
        }
        public async Task<(IEnumerable<TradeSet>, string? nextCursor)> GetTradeSetsAsync(string? cursor, int pageSize) 
        {
            var query = _dbContext.TradeSets.Include(q=>q.Trades).OrderByDescending(ts => ts.ImportedDate).AsQueryable();

            if (!string.IsNullOrEmpty(cursor) && DateTime.TryParse(cursor, out var lastDate))
            {
                query = query.Where(ts => ts.ImportedDate < lastDate.ToUniversalTime());
            }

            var tradeSets =  await query.Take(pageSize).ToListAsync();
            var nextCursor = tradeSets.Any() ? tradeSets.Last().ImportedDate.ToString("O") : null;
            return (tradeSets, nextCursor);
        }
        private static List<string> ParseCsvLine(string line)
        {
            var values = new List<string>();
            var pattern = @"(?:^|,)(\"".*?\"")|([^,]+)";
            var matches = Regex.Matches(line, pattern);

            foreach (Match match in matches)
            {
                var value = match.Groups[1].Value;
                if (!string.IsNullOrEmpty(value))
                {
                    // Remove surrounding quotes and trim
                    values.Add(value.Trim('"'));
                }
                else
                {
                    values.Add(match.Groups[2].Value.Trim());
                }
            }

            return values;
        }
        private static string RemoveCurrencySymbol(string value)
        {
            if (string.IsNullOrEmpty(value)) return value;

            // Remove any currency symbols like '$' or ',' and trim extra spaces
            return value.Replace("$", "").Replace(",", "").Trim();
        }

        public async Task<Result<object>> ImportTradeSetsAsync(string? file, string name, string mapping, bool ignoreFirstRow)
        {
            var importDate = DateTime.UtcNow;

            var tradeSet = new TradeSet(name)
            {
                ImportedDate = importDate
            };

            var mappingColumns = mapping.Split(',').Select(m => m.Trim()).ToArray();

            if (string.IsNullOrEmpty(file))
            {
                return new Result<object>(false, "No file provided", name);
            }

            try
            {
                using (var reader = new StringReader(file))
                {
                    string? line;
                    bool isFirstRow = true;

                    while ((line = reader.ReadLine()) != null)
                    {
                        if (isFirstRow && ignoreFirstRow)
                        {
                            isFirstRow = false;
                            continue;
                        }

                        var values = ParseCsvLine(line);

                        var trade = new Dictionary<string, string>();

                        for (int i = 0; i < mappingColumns.Length; i++)
                        {
                            var key = mappingColumns[i];
                            var value = i < values.Count() ? values[i].Trim() : string.Empty;

                            if (!string.IsNullOrEmpty(value))
                            {
                                trade[key] = value;
                            }
                        }

                        var tradeSymbol = trade.GetValueOrDefault("Symbol");
                        if (!string.IsNullOrEmpty(tradeSymbol))
                        {
                            var tradeToAdd = new Trade
                            {
                                Symbol = tradeSymbol,
                                StartDate = DateTime.TryParse(trade.GetValueOrDefault("Entry Date"), out var entryDate) ? entryDate.ToUniversalTime() : importDate,
                                EndDate = DateTime.TryParse(trade.GetValueOrDefault("Exit Date"), out var exitDate) ? exitDate.ToUniversalTime() : importDate,
                                Entry = decimal.TryParse(RemoveCurrencySymbol(trade.GetValueOrDefault("Entry")), out var entry) ? entry : 0,
                                TG1 = decimal.TryParse(RemoveCurrencySymbol(trade.GetValueOrDefault("TG1")), out var tg1) ? tg1 : 0,
                                TG2 = decimal.TryParse(RemoveCurrencySymbol(trade.GetValueOrDefault("TG2")), out var tg2) ? tg2 : 0,
                                StopLoss = decimal.TryParse(RemoveCurrencySymbol(trade.GetValueOrDefault("SL")), out var sl) ? sl : 0,
                                BasedOn = ChartTime.Weekly
                            };
                            tradeSet.Trades.Add(tradeToAdd);
                        }
                    }
                }
                _dbContext.Add(tradeSet);

                await _dbContext.SaveChangesAsync();

                return new Result<object>(true,"Trades imported successfully",tradeSet);
            }
            catch (Exception ex)
            {                
                return new Result<object>(false, $"Error importing trades: {ex.Message}", name);
            }
        }

        public async Task<(IEnumerable<WatchList>, string? nextCursor)> GetWatchListsAsync(string? cursor, int pageSize)
        {
            var query = _dbContext.WatchList.Include(x=>x.Agent).Include(q => q.Symbols).OrderByDescending(ts => ts.ImportedDate).AsQueryable();

            if (!string.IsNullOrEmpty(cursor) && DateTime.TryParse(cursor, out var lastDate))
            {
                query = query.Where(ts => ts.ImportedDate < lastDate.ToUniversalTime());
            }

            var watchLists = await query.Take(pageSize).ToListAsync();
            var nextCursor = watchLists.Any() ? watchLists.Last().ImportedDate.ToString("O") : null;
            return (watchLists, nextCursor);
        }

        public async Task<IEnumerable<WatchListSymbol>> GetWatchListSymbolsAsync(Guid watchListId) {
            var query = _dbContext.Symbols.Where(x => x.WatchListId == watchListId).AsQueryable();
            return await query.ToListAsync();            
        }

        public async Task<Result<object>> ImportWatchListAsync(string? file, string name, string mapping, bool ignoreFirstRow)
        {
            var importDate = DateTime.UtcNow;

            var watchList = new WatchList(name)
            {
                ImportedDate = importDate
            };

            var mappingColumns = mapping.Split(',').Select(m => m.Trim()).ToArray();

            if (string.IsNullOrEmpty(file))
            {
                return new Result<object>(false, "No file provided", name);
            }

            try
            {
                using (var reader = new StringReader(file))
                {
                    string? line;
                    bool isFirstRow = true;

                    while ((line = reader.ReadLine()) != null)
                    {
                        if (isFirstRow && ignoreFirstRow)
                        {
                            isFirstRow = false;
                            continue;
                        }

                        var values = ParseCsvLine(line);

                        var importedSymbol = new Dictionary<string, string>();

                        for (int i = 0; i < mappingColumns.Length; i++)
                        {
                            var key = mappingColumns[i];
                            var value = i < values.Count() ? values[i].Trim() : string.Empty;

                            if (!string.IsNullOrEmpty(value))
                            {
                                importedSymbol[key] = value;
                            }
                        }

                        var watchListSymbol = importedSymbol.GetValueOrDefault("Symbol")?.Split(":");
                        if (watchListSymbol?.Length==2)
                        {
                            var symbolToAdd = new WatchListSymbol
                            {
                                Symbol = watchListSymbol[1],
                                Market = watchListSymbol[0],
                                CompanyName = string.Empty
                            };
                            watchList.Symbols.Add(symbolToAdd);
                        }
                    }
                }
                _dbContext.Add(watchList);

                await _dbContext.SaveChangesAsync();

                return new Result<object>(true, "WatchList imported successfully", watchList);
            }
            catch (Exception ex)
            {
                return new Result<object>(false, $"Error importing watch list symbols: {ex.Message}", name);
            }
        }
        private DateTime ConverUnixTimeStampToDateTime(long unixTimestampMilliseconds)
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(unixTimestampMilliseconds);
            return dateTimeOffset.UtcDateTime; ;
        }
        public async Task RunAgentForWatchlist(Guid watchListId)
        {
            var watchList= await _dbContext
                    .WatchList
                    .Include(x => x.Symbols)
                    .Include(x => x.Agent)
                        .ThenInclude(a => a.ApiProvider)
                    .Where(x=>x.Id == watchListId)
                    .FirstAsync();
            List<DateTime> CallTimestamps = new List<DateTime>();
            
            foreach (var symbol in 
                        watchList.Symbols.OrderBy(x=>x.Symbol)
                            .Where(i=> string.Compare(i.Symbol, "MCHP", StringComparison.Ordinal) >= 0))
            {
                var httpClient = new HttpClient();
                var lastRun = symbol.LastImportedDate?.ToString("yyyy-MM-dd") ?? "2022-01-01";
                if (lastRun == "0001-01-01")
                    lastRun = "2005-01-01";
                var thisweek = DateTime.Now.ToString("yyyy-MM-dd");
                var done = false;
                try
                {
                    var priceHistory = new List<EquityPriceHistory>();
                    var formattedUrl = string.Format(watchList.Agent.ApiProvider.BaseUrl, symbol.Symbol, lastRun, thisweek, "");
                    if (CallTimestamps.Count >= 5)
                    {
                        Task.Delay(TimeSpan.FromMinutes(1)).Wait();
                        CallTimestamps.Clear();
                    }
                    CallTimestamps.Add(DateTime.UtcNow);
                    HttpResponseMessage response = await httpClient.GetAsync(formattedUrl);
                    response.EnsureSuccessStatusCode();

                    string responseData = await response.Content.ReadAsStringAsync();
                    var apiResponse = JsonSerializer.Deserialize<PolyGonIOApiResponse>(responseData);
                    if (apiResponse.results.Count > 0)
                    {
                        thisweek = ConverUnixTimeStampToDateTime(apiResponse.results[0].t).ToString("yyyy-MM-dd");
                    }
                    foreach (var item in apiResponse.results)
                    {
                        priceHistory.Add(new EquityPriceHistory()
                        {
                            Symbol = symbol.Symbol,
                            Volume = item.v,
                            VolumeWeighted = item.vw,
                            Open = item.o,
                            Close = item.c,
                            High = item.h,
                            Low = item.l,
                            PriceDate = ConverUnixTimeStampToDateTime(item.t),
                            RawPriceDatee = item.t,
                            ChartTime = ChartTime.Weekly,
                        });
                    }
                    var rangeToAdd = priceHistory.DistinctBy(x=>x.PriceDate).ToList();
                    await _dbContext.AddRangeAsync(rangeToAdd);
                    symbol.LastImportedDate = DateTime.UtcNow;
                    await _dbContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    return;
                }

            }
            await _dbContext.SaveChangesAsync();
        }
    }
}
