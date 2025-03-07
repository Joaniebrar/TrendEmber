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

        private async Task RunAgentForWatchlistAsync(Guid watchListId, string runWeek, long unixtimestamp)
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
                var done = false;
                try
                {
                    var priceHistory = new List<EquityPriceHistory>();
                    var formattedUrl = string.Format(watchList.Agent.ApiProvider.BaseUrl, symbol.Symbol, runWeek, runWeek, "w_n4F71ZNG27db1zQYcfiGBjpgF3jAJk");
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

        public async Task<List<TradeSetup>> IdentifyTradeSetupsAsync(WatchListSymbol symbol, long priceDate)
        {
            var tradeSetups = new List<TradeSetup>();

            var history = await _dbContext.EquityPrices
                .Where(h => h.Symbol == symbol.Symbol)
                .OrderBy(h => h.PriceDate)
                .ToListAsync();

            for (int i = 1; i < history.Count; i++)
            {
                var prev = history[i - 1];
                var curr = history[i];

                // Setup 1: FullBar with Z-Score between 1.5 and 5, followed by Hammer or Doji
                if (prev.Shape == CandleShape.FullBar && prev.RangeZScore is >= 1.5 and <= 5 &&
                    prev.Open > prev.Close &&
                    (curr.Shape == CandleShape.Hammer || curr.Shape == CandleShape.Doji))
                {
                    tradeSetups.Add(new TradeSetup
                    {
                        Id = Guid.NewGuid(),
                        PriceHistoryId = curr.Id,
                        PriceHistory = curr,
                        TradeType = TradeType.BigBarPause
                    });
                }

                // Setup 2: FullBar with Z-Score between -0.5 and 1.4, prev closes lower, curr closes higher
                if (prev.Shape == CandleShape.FullBar && prev.RangeZScore is >= -0.5 and <= 1.4 && prev.Close < prev.Open &&
                    (curr.Shape == CandleShape.FullBar || curr.Shape == CandleShape.TailBar) && curr.RangeZScore is >= -0.5 and <= 1.4 && curr.Close > curr.Open)
                {
                    tradeSetups.Add(new TradeSetup
                    {
                        Id = Guid.NewGuid(),
                        PriceHistoryId = curr.Id,
                        PriceHistory = curr,
                        TradeType = TradeType.Engulfing
                    });
                }

                // Setup 3: Doji or Hammer followed by FullBar closing higher
                if ((prev.Shape == CandleShape.Doji || prev.Shape == CandleShape.Hammer) &&
                    prev.RangeZScore is >= -0.5 and <= 1.4 &&
                    curr.Shape == CandleShape.FullBar && curr.RangeZScore is >= -0.5 and <= 1.4 && curr.Close > prev.Close)
                {
                    tradeSetups.Add(new TradeSetup
                    {
                        Id = Guid.NewGuid(),
                        PriceHistoryId = curr.Id,
                        PriceHistory = curr,
                        TradeType = TradeType.DojiConfirmed
                    });
                }

                // Setup 4: TailBar with Z-Score 1-1.5 and closing lower, followed by FullBar
                if (prev.Shape == CandleShape.TailBar && prev.RangeZScore is >= 1.0 and <= 1.5 && prev.Close < prev.Open &&
                    curr.Shape == CandleShape.FullBar && curr.RangeZScore is >= -0.5 and <= 1.4 &&
                    curr.Open < prev.Close && curr.Close >= prev.Low)
                {
                    tradeSetups.Add(new TradeSetup
                    {
                        Id = Guid.NewGuid(),
                        PriceHistoryId = curr.Id,
                        PriceHistory = curr,
                        TradeType = TradeType.ContainedTailBar
                    });
                }
            }

            // Setup 5: Three TailBars, Dojis, or Hammers between two wavepoints
            var wavePoints = await _dbContext.WavePoints
                .Where(w => w.PriceHistory.Symbol == symbol.Symbol)
                .OrderBy(w => w.PriceHistory.PriceDate)
                .ToListAsync();

            for (int i = 1; i < wavePoints.Count; i++)
            {
                var startWave = wavePoints[i - 1];
                var endWave = wavePoints[i];

                var barsBetween = history.Where(h => h.PriceDate > startWave.PriceHistory.PriceDate &&
                                                     h.PriceDate < endWave.PriceHistory.PriceDate)
                                         .ToList();

                var qualifyingBars = barsBetween.Where(h =>
                    (h.Shape == CandleShape.TailBar || h.Shape == CandleShape.Doji || h.Shape == CandleShape.Hammer) &&
                    h.RangeZScore > 1.3 && h.Low < history[history.IndexOf(h) - 1].Low).ToList();

                if (qualifyingBars.Count >= 3)
                {
                    tradeSetups.Add(new TradeSetup
                    {
                        Id = Guid.NewGuid(),
                        PriceHistoryId = qualifyingBars.Last().Id,
                        PriceHistory = qualifyingBars.Last(),
                        TradeType = TradeType.ThreeTails
                    });
                }
            }

            await _dbContext.TradeSetups.AddRangeAsync(tradeSetups);
            await _dbContext.SaveChangesAsync();

            return tradeSetups;
        }

        public async Task DetectTradeSetupsAsync(long priceDate)
        {
            var symbols = _dbContext.Symbols.ToDictionary(stat => stat.Symbol);
            foreach (var symbol in symbols)
            {
                var trades = await IdentifyTradeSetupsAsync(symbol.Value, priceDate);
            }

        }

        public async Task RunWeeklyImportAsync() 
        {
            var lastImport = _dbContext.WeeklyImports
                .OrderByDescending(w => w.RunFor)
                .FirstOrDefault();
            var watchList = _dbContext.WatchList.First();
            var weeklyImport = 1740891600000;
            var weekInQuestion = "2025-03-06";
            await RunAgentForWatchlistAsync(watchList.Id, weekInQuestion, weeklyImport);
            await DetectTradeSetupsAsync(weeklyImport);
        }
    }
}
