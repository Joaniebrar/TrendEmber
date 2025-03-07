using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Net.Http;
using System.Text.Json;
using System.Text.RegularExpressions;
using TrendEmber.Core;
using TrendEmber.Core.Trends;
using TrendEmber.Data;

namespace TrendEmber.Service
{
    public class EquityStats
    {
        public string Symbol { get; set; }
        public double Mean { get; set; }
        public double StandardDeviation { get; set; }
    }

    public class TradeService : ITradeService
    {
        private TrendsDbContext _dbContext;
        public TradeService(TrendsDbContext trendsDbContext)
        {
            _dbContext = trendsDbContext;
        }
        public async Task<(IEnumerable<TradeSet>, string? nextCursor)> GetTradeSetsAsync(string? cursor, int pageSize)
        {
            var query = _dbContext.TradeSets.Include(q => q.Trades).OrderByDescending(ts => ts.ImportedDate).AsQueryable();

            if (!string.IsNullOrEmpty(cursor) && DateTime.TryParse(cursor, out var lastDate))
            {
                query = query.Where(ts => ts.ImportedDate < lastDate.ToUniversalTime());
            }

            var tradeSets = await query.Take(pageSize).ToListAsync();
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

                return new Result<object>(true, "Trades imported successfully", tradeSet);
            }
            catch (Exception ex)
            {
                return new Result<object>(false, $"Error importing trades: {ex.Message}", name);
            }
        }

        public async Task<(IEnumerable<WatchList>, string? nextCursor)> GetWatchListsAsync(string? cursor, int pageSize)
        {
            var query = _dbContext.WatchList.Include(x => x.Agent).Include(q => q.Symbols).OrderByDescending(ts => ts.ImportedDate).AsQueryable();

            if (!string.IsNullOrEmpty(cursor) && DateTime.TryParse(cursor, out var lastDate))
            {
                query = query.Where(ts => ts.ImportedDate < lastDate.ToUniversalTime());
            }

            var watchLists = await query.Take(pageSize).ToListAsync();
            var nextCursor = watchLists.Any() ? watchLists.Last().ImportedDate.ToString("O") : null;
            return (watchLists, nextCursor);
        }

        public async Task<IEnumerable<WatchListSymbol>> GetWatchListSymbolsAsync(Guid watchListId)
        {
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
                        if (watchListSymbol?.Length == 2)
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
        private DateTime ConverUnixTimeStampToDateTime(long unixTimestampMilliseconds) =>
            DateTimeOffset.FromUnixTimeMilliseconds(unixTimestampMilliseconds).UtcDateTime;

        public async Task RunAgentForWatchlist(Guid watchListId)
        {
            var watchList = await _dbContext
                    .WatchList
                    .Include(x => x.Symbols)
                    .Include(x => x.Agent)
                        .ThenInclude(a => a.ApiProvider)
                    .Where(x => x.Id == watchListId)
                    .FirstAsync();
            List<DateTime> CallTimestamps = new List<DateTime>();

            foreach (var symbol in
                        watchList.Symbols.OrderBy(x => x.Symbol))
            {
                var httpClient = new HttpClient();
                var lastRun = symbol.LastImportedDate?.ToString("yyyy-MM-dd") ?? "2022-01-01";
                if (lastRun == "0001-01-01")
                    lastRun = "2022-01-01";
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
                    var rangeToAdd = priceHistory.DistinctBy(x => x.PriceDate).ToList();
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

        public void CalculateMeanAndStandardDeviation()
        {
            var calculatedResults = _dbContext.EquityPrices
                .AsEnumerable()
                .GroupBy(e => e.Symbol)
                .Select(g =>
                {
                    var ranges = g.Select(e => (double)(e.High - e.Low)).ToList();
                    var mean = ranges.Average();
                    var stdDev = Math.Sqrt(ranges.Sum(p => Math.Pow(p - mean, 2)) / ranges.Count);

                    return new EquityStats
                    {
                        Symbol = g.Key,
                        Mean = mean,
                        StandardDeviation = stdDev
                    };
                })
                .ToDictionary(stat => stat.Symbol);
            foreach (var symbol in _dbContext.Symbols)
            {
                var symbolStats = calculatedResults[symbol.Symbol];
                symbol.MeanRange = symbolStats.Mean;
                symbol.StandardDeviation = symbolStats.StandardDeviation;
            }
            _dbContext.SaveChanges();
        }



        public async Task FindPeaksAndTroughsForWatchListAsync()
        {
            var symbols = _dbContext.Symbols.ToDictionary(stat => stat.Symbol);
            foreach (var symbol in symbols)
            {
                var prices = await _dbContext.EquityPrices
                    .Where(x => x.Symbol.Equals(symbol.Value.Symbol)).OrderBy(x => x.PriceDate).ToListAsync();
                var waves = await FindPeaksAndTroughs(prices, symbol.Value.Id);
                foreach (var wave in waves)
                {
                    if (!_dbContext.WavePoints.Any(x => x.PriceHistoryId == wave.PriceHistoryId))
                    {
                        await _dbContext.WavePoints.AddAsync(wave);
                    }
                }
                await _dbContext.SaveChangesAsync();
            }

        }
        public async Task<List<WavePoint>> FindPeaksAndTroughs(List<EquityPriceHistory> prices, Guid symbolId, int lookback = 2)
        {
            var peaksAndTroughs = new List<WavePoint>();

            for (int i = lookback; i < prices.Count - lookback; i++)
            {
                decimal currentHigh = prices[i].High;
                decimal currentLow = prices[i].Low;

                bool isPeak = prices.Skip(i - lookback).Take(lookback * 2 + 1)
                    .All(p => p.High <= currentHigh);

                bool isTrough = prices.Skip(i - lookback).Take(lookback * 2 + 1)
                    .All(p => p.Low >= currentLow);

                if (isPeak)
                    peaksAndTroughs.Add(new WavePoint
                    {
                        SymbolId = symbolId,
                        PriceDate = prices[i].PriceDate,
                        Type = WaveType.Peak,
                        Price = currentHigh,
                        PriceHistoryId = prices[i].Id
                    });

                if (isTrough)
                    peaksAndTroughs.Add(new WavePoint
                    {
                        SymbolId = symbolId,
                        PriceDate = prices[i].PriceDate,
                        Type = WaveType.Trough,
                        Price = currentLow,
                        PriceHistoryId = prices[i].Id
                    });
            }

            return peaksAndTroughs;
        }

        public async Task DetectGapsAsync()
        {
            var symbols = await _dbContext.Symbols.ToDictionaryAsync(stat => stat.Symbol);
            foreach (var symbol in symbols)
            {
                await DetectGapsForEqutiyAsync(symbol.Value);
            }
            foreach (var symbol in symbols)
            {
                await IdentifyFilledGapsForEquityAsync(symbol.Value);
            }
        }
        public async Task DetectGapsForEqutiyAsync(WatchListSymbol symbol)
        {
            var history = await _dbContext.EquityPrices
                .Where(h => h.Symbol == symbol.Symbol)
                .OrderBy(h => h.PriceDate)
                .ToListAsync();

            var gaps = new List<PriceGapEvent>();

            for (int i = 1; i < history.Count; i++)
            {
                var prev = history[i - 1];
                var curr = history[i];

                // Apply 2% gap threshold (both up and down)
                if (curr.Open > prev.Close * 1.02m) // 2% up gap
                {                    
                    gaps.Add(new PriceGapEvent
                    {
                        Id = Guid.NewGuid(),
                        ClosingEquityPriceHistoryId = prev.Id,
                        OpeningEquityPriceHistoryId = curr.Id,
                        Direction = GapDirection.GapUp,
                        GapFilledPriceHistoryId = null,
                        Symbol = symbol.Symbol,
                        Close = prev.Close,
                        Open = curr.Open,
                        
                    });
                }
                else if (curr.Open < prev.Close * 0.98m) // 2% down gap
                {
                    gaps.Add(new PriceGapEvent
                    {
                        Id = Guid.NewGuid(),
                        ClosingEquityPriceHistoryId = prev.Id,
                        OpeningEquityPriceHistoryId = curr.Id,
                        Direction = GapDirection.GapDown,
                        GapFilledPriceHistoryId = null,
                        Symbol = symbol.Symbol,
                        Close = prev.Close,
                        Open = curr.Open,
                    });
                }
            }

            foreach (var gap in gaps)
            {
                if (!_dbContext.PriceGapEvents
                    .Any(x => x.ClosingEquityPriceHistoryId == gap.ClosingEquityPriceHistoryId))
                {
                    await _dbContext.PriceGapEvents.AddAsync(gap);
                }
            }
            await _dbContext.SaveChangesAsync();

        }

        public async Task IdentifyFilledGapsForEquityAsync(WatchListSymbol symbol)
        {
            var gaps = await _dbContext.PriceGapEvents
                .Where(g => g.GapFilledPriceHistoryId == null)
                .Include(g => g.OpeningPriceHistory)  
                .ToListAsync();

            var subsequentPrices = await _dbContext.EquityPrices
                .Where(h => h.Symbol == symbol.Symbol)
                .OrderBy(h => h.PriceDate)
                .ToListAsync();
            
            foreach (var gap in gaps)
            {
                var subsequentPriceEntries = subsequentPrices
                    .Where(h => h.PriceDate > gap.OpeningPriceHistory.PriceDate)
                    .ToList();

                foreach (var price in subsequentPriceEntries)
                {
                    if (gap.Direction == GapDirection.GapUp && price.Close < gap.Open) 
                    {
                        gap.GapFilledPriceHistoryId = price.Id;

                        break; 
                    }
                    else if (gap.Direction == GapDirection.GapDown && price.Close > gap.Close) 
                    {
                        gap.GapFilledPriceHistoryId = price.Id;
                        break; 
                    }
                }
            }
            
            await _dbContext.SaveChangesAsync();
        }

        public async Task SimulateTrades()
        {
            var symbols = _dbContext.Symbols.ToDictionary(stat => stat.Symbol);
            foreach (var symbol in symbols)
            {
                var tradeSetups = _dbContext.TradeSetups.Include(x => x.PriceHistory)
                        .Where(x => x.PriceHistory.Symbol == symbol.Value.Symbol)
                        .OrderBy(x => x.PriceHistory.PriceDate).ToList();
                var resistancePoints = _dbContext.ResistancePoints
                        .Where(x => x.Symbol == symbol.Value.Symbol).ToList();
                foreach (var setup in tradeSetups)
                {

                    var tradeSimulation = new TradeSetupSimulation();
                    tradeSimulation.Symbol = symbol.Value.Symbol;
                    tradeSimulation.Version = "v1";
                    tradeSimulation.TradeSetupId = setup.Id;
                    tradeSimulation.Entry = setup.PriceHistory.Close;
                    tradeSimulation.TradeDate = setup.PriceHistory.PriceDate;

                    var nearestResistances = resistancePoints
                        .Where(rp => rp.Resistance.HasValue
                                     && rp.Resistance.Value > tradeSimulation.Entry
                                     && rp.StartDate <= tradeSimulation.TradeDate
                                     && (!rp.EndDate.HasValue || rp.EndDate > tradeSimulation.TradeDate))
                        .OrderBy(rp => rp.Resistance)
                        .Take(2)
                        .ToList();
                    var nearestSupport = resistancePoints
                        .Where(rp => rp.Resistance.HasValue
                                     && rp.Resistance.Value < tradeSimulation.Entry
                                     && rp.StartDate <= tradeSimulation.TradeDate
                                     && (!rp.EndDate.HasValue || rp.EndDate > tradeSimulation.TradeDate))
                        .OrderBy(rp => rp.Resistance)
                        .Take(2)
                        .ToList();
                    tradeSimulation.FirstResistance = nearestResistances.ElementAtOrDefault(0)?.Resistance;
                    tradeSimulation.SecondResistance = nearestResistances.ElementAtOrDefault(1)?.Resistance;
                    tradeSimulation.FirstSupport = nearestSupport.ElementAtOrDefault(0)?.Resistance;
                    tradeSimulation.SecondSupport = nearestSupport.ElementAtOrDefault(1)?.Resistance;
                    await _dbContext.AddAsync(tradeSimulation);
                }
            }
            await _dbContext.SaveChangesAsync();

        }

        public static decimal CalculateProportionalZ(decimal x, decimal y)
        {
            decimal percentageIncrease = (y - x) / x;
            decimal z = y + (percentageIncrease * y);
            return z;
        }

        public async Task CalculateSimulationResultsV2()
        {
            foreach (var simulation in _dbContext.TradeSetupSimulations)
            {
                if (simulation.FirstResistance >= simulation.Entry * 1.065m)
                {
                    simulation.Version = "v2";
                    simulation.TG1 = simulation.FirstResistance;
                    simulation.TG2 = simulation.SecondResistance.HasValue ? simulation.SecondResistance : simulation.FirstResistance*1.05m;
                    simulation.TG1Percentage = ((simulation.TG1 - simulation.Entry) / simulation.Entry) * 100;
                    simulation.TG2Percentage = ((simulation.TG2 - simulation.Entry) / simulation.Entry) * 100;
                    var declinePercentage = ((simulation.TG1 - simulation.Entry) / simulation.TG1) * 100;
                    simulation.SL = decimal.Max(simulation.Entry.Value * (1 - (declinePercentage.Value / 100)),
                            simulation.FirstSupport.HasValue ? simulation.FirstSupport.Value : decimal.MinValue);
                    simulation.SLPercentage = ((simulation.Entry - simulation.SL) / simulation.Entry) * 100;

                }
            }
            await _dbContext.SaveChangesAsync();
        }

        public async Task CalculateSimulationResults()
        {
            foreach (var simulation in _dbContext.TradeSetupSimulations)
            {
                if (simulation.FirstResistance >= simulation.Entry * 1.065m)
                {
                    simulation.Version = "v3";
                    simulation.TG1 = simulation.FirstResistance;
                    simulation.TG2 = simulation.Entry * (1 + ((simulation.TG1 - simulation.Entry) / simulation.Entry) * 1.10m);
                    simulation.TG1Percentage = ((simulation.TG1 - simulation.Entry) / simulation.Entry) * 100;
                    simulation.TG2Percentage = ((simulation.TG2 - simulation.Entry) / simulation.Entry) * 100;
                    var declinePercentage = ((simulation.TG1 - simulation.Entry) / simulation.TG1) * 100;
                    simulation.SL = decimal.Max(simulation.Entry.Value * (1 - (declinePercentage.Value / 100)),
                            simulation.FirstSupport.HasValue ? simulation.FirstSupport.Value : decimal.MinValue);
                    simulation.SLPercentage = ((simulation.Entry - simulation.SL) / simulation.Entry) * 100;

                }
            }
            await _dbContext.SaveChangesAsync();
        }

        public async Task<(decimal? SelectedValue, DateTime PriceDate)> GetSelectedValueAndDateAsync(TradeSetupSimulation simulation)
        {
            var equityPrices = await _dbContext.EquityPrices.AsNoTracking()
                .Where(x => x.Symbol == simulation.Symbol && x.PriceDate > simulation.TradeDate)
                .Select(x => new { x.PriceDate,x.High,x.Low})
                .OrderBy(x => x.PriceDate)
                .ToListAsync(); // Retrieve the list of equity prices asynchronously

            var result = equityPrices
                .Aggregate((Reached: (decimal?)null, FoundTG2: false, PriceDate: (DateTime?)null), (acc, price) =>
                {
                    if (price.Low <= simulation.TG2 && price.High >= simulation.TG2 && !acc.FoundTG2)
                    {
                        acc.FoundTG2 = true;
                        acc.Reached = simulation.TG2;
                        acc.PriceDate = price.PriceDate;
                    }

                    if (price.Low <= simulation.SL && price.High >= simulation.SL)
                    {
                        acc.Reached = simulation.SL;
                        acc.PriceDate = price.PriceDate;
                        return acc; // Stop further processing once SL is found
                    }

                    return acc;
                });

            var selectedValue = result.Reached ?? simulation.TG2;
            var selectedPriceDate = result.PriceDate ?? default(DateTime); // Ensure default if PriceDate is null

            return (selectedValue, selectedPriceDate);
        }

        public async Task<decimal> CalculatePercentageDifferenceAsync(decimal entry, decimal exit)
        {
            // Simulate some async work if needed (e.g., data fetching, external calls, etc.)
            await Task.Delay(100); // This is just for demonstration purposes and can be removed.

            // Calculate the percentage difference
            decimal percentageDifference = ((exit - entry) / entry) * 100;

            return percentageDifference;
        }


        public async Task CalculateExits()
        {
            foreach (var simulation in _dbContext.TradeSetupSimulations.ToList())
            {
                if (simulation.Version == "v3")
                {
                    var result = await GetSelectedValueAndDateAsync(simulation);
                    simulation.Exit = result.SelectedValue;
                    simulation.ExitDate = result.PriceDate;
                    simulation.ExitPercentage = await CalculatePercentageDifferenceAsync(simulation.Entry.Value, simulation.Exit.Value);
                }
            }
            await _dbContext.SaveChangesAsync();
        }

        public async Task RunAgentForWeek(Guid watchListId)
        {
            var watchList = await _dbContext
                    .WatchList
                    .Include(x => x.Symbols)
                    .Include(x => x.Agent)
                        .ThenInclude(a => a.ApiProvider)
                    .Where(x => x.Id == watchListId)
                    .FirstAsync();
            List<DateTime> CallTimestamps = new List<DateTime>();

            foreach (var symbol in
                        watchList.Symbols.OrderBy(x => x.Symbol))
            {
                var httpClient = new HttpClient();
                var lastRun = symbol.LastImportedDate?.ToString("yyyy-MM-dd") ?? "2022-01-01";
                if (lastRun == "0001-01-01")
                    lastRun = "2022-01-01";
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
                    var rangeToAdd = priceHistory.DistinctBy(x => x.PriceDate).ToList();
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
