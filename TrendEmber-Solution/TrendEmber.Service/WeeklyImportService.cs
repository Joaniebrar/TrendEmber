using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using TrendEmber.Core;
using TrendEmber.Core.Trends;
using TrendEmber.Data;

namespace TrendEmber.Service
{
    public class WeeklyImportService : IWeeklyImportService
    {
        private TrendsDbContext _dbContext;
        public WeeklyImportService(TrendsDbContext trendsDbContext)
        {
            _dbContext = trendsDbContext;
        }

        private DateTime ConverUnixTimeStampToDateTime(long unixTimestampMilliseconds) =>
            DateTimeOffset.FromUnixTimeMilliseconds(unixTimestampMilliseconds).UtcDateTime;

        private async Task RunAgentForWatchlistAsync(Guid watchListId, string runWeekStart, string runWeekEnd, long unixtimestamp)
        {

            var watchList = await _dbContext
                    .WatchList
                    .Include(x => x.Symbols)
                    .Include(x => x.Agent)
                        .ThenInclude(a => a.ApiProvider)
                    .Where(x => x.Id == watchListId)
                    .FirstAsync();

            var recordsToDelete = _dbContext.EquityPrices.Where(ep => ep.RawPriceDatee == unixtimestamp);
            _dbContext.EquityPrices.RemoveRange(recordsToDelete);
            await _dbContext.SaveChangesAsync();

            foreach (var symbol in watchList.Symbols
                .Where(x => x.Symbol != "SQ")
                .OrderBy(x => x.Symbol))
            {
                var httpClient = new HttpClient();
                try
                {
                    var priceHistory = new List<EquityPriceHistory>();
                    var formattedUrl = string.Format(watchList.Agent.ApiProvider.BaseUrl, symbol.Symbol, runWeekStart, runWeekEnd, "");
                    HttpResponseMessage response = await httpClient.GetAsync(formattedUrl);
                    response.EnsureSuccessStatusCode();

                    string responseData = await response.Content.ReadAsStringAsync();
                    var apiResponse = JsonSerializer.Deserialize<PolyGonIOApiResponse>(responseData);
                    foreach (var item in apiResponse.results)
                    {
                        if (item.t == unixtimestamp)
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
                                RangeZScore = CandleStickAnalyzer.CalculateZScore(item.h, item.l, symbol.MeanRange.Value, symbol.StandardDeviation.Value),
                                Shape = CandleStickAnalyzer.CalculateCandleShape(item.o, item.c, item.h, item.l)
                            });
                        }
                    }
                    await _dbContext.AddRangeAsync(priceHistory);
                    symbol.LastImportedDate = GetLastSaturday().AddDays(7);
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

        private DateTime GetLastSaturday() {
            DateTime currentDate = DateTime.UtcNow;

            // Calculate the days to subtract
            int daysToSubtract = (int)currentDate.DayOfWeek - (int)DayOfWeek.Saturday;
            if (daysToSubtract < 0)
            {
                daysToSubtract += 7; // If today is Sunday (or later), we need to subtract extra days to get to the last Saturday
            }

            return currentDate.AddDays(-daysToSubtract);
        }

        public async Task<List<TradeSetup>> IdentifyTradeSetupsAsync(WatchListSymbol symbol, long? priceDate)
        {
            var tradeSetups = new List<TradeSetup>();

            // Query equity prices
            var equityPricesQuery = _dbContext.EquityPrices
                .Where(h => h.Symbol == symbol.Symbol);

            if (priceDate.HasValue)
            {
                equityPricesQuery = equityPricesQuery.Where(h => h.RawPriceDatee == priceDate.Value);
            }

            var equityPrices = await equityPricesQuery
                .OrderBy(h => h.PriceDate)
                .AsNoTracking()
                .ToListAsync();

            for (var i = 0; i < equityPrices.Count; i++)
            {
                var currentPrice = equityPrices[i];
                List<EquityPriceHistory> pricesBetween;

                pricesBetween = new List<EquityPriceHistory> { currentPrice };
                if (i > 0)
                {
                    pricesBetween.Insert(0, equityPrices[i - 1]); // Add previous price if exists
                }

                var lastPeakWavePoint = await _dbContext.WavePoints
                    .Where(w => w.SymbolId == symbol.Id && w.PriceDate < currentPrice.PriceDate && w.Type == WaveType.Peak)
                    .OrderByDescending(w => w.PriceDate)
                    .FirstOrDefaultAsync();

                // Analyze trade setup
                var tradeSetup = await TradeSetupAnalyzer.Analyze(pricesBetween, lastPeakWavePoint);
                if (tradeSetup != null)
                {
                    tradeSetups.Add(tradeSetup);
                }
            }

            // Batch insert trade setups
            if (tradeSetups.Count > 0)
            {
                await _dbContext.TradeSetups.AddRangeAsync(tradeSetups);
                await _dbContext.SaveChangesAsync();
            }

            return tradeSetups;
        }


        public async Task DetectTradeSetupsAsync(long? priceDate = null)
        {
            var symbols = _dbContext.Symbols.ToDictionary(stat => stat.Symbol);
            foreach (var symbol in symbols)
            {
                var trades = await IdentifyTradeSetupsAsync(symbol.Value, priceDate);
            }

        }

        public async Task RunWeeklyImportAsync() 
        {            
            var watchList = _dbContext.WatchList.First();
            var weeklyImport = 1740891600000;
            var weekInQuestionStart = "2025-03-02";
            var weekInQuestionEnd = "2025-03-08";
            //await RunAgentForWatchlistAsync(watchList.Id, weekInQuestionStart, weekInQuestionEnd, weeklyImport);
            await DetectTradeSetupsAsync();
        }
    }
}
