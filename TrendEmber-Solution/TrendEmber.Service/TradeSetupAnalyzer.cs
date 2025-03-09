using TrendEmber.Core;
using TrendEmber.Core.Trends;

namespace TrendEmber.Service
{

    public static class TradeSetupAnalyzer
    {
        private static Func<EquityPriceHistory, bool> isValidThreeTailsShape = item =>
            (item.Shape == CandleShape.TailBar ||
             item.Shape == CandleShape.Doji ||
             item.Shape == CandleShape.Hammer) &&
            item.RangeZScore > 1.3;

        public static TradeSetup? IsBigBarPauseTradeSetup(EquityPriceHistory prev, EquityPriceHistory curr)
        {            
            if (prev.Shape == CandleShape.FullBar && prev.RangeZScore is >= 1.12 and <= 5 &&
                prev.Open > prev.Close &&
                (curr.Shape == CandleShape.Hammer || curr.Shape == CandleShape.Doji))
            {
                return new TradeSetup
                {
                    PriceHistoryId = curr.Id,
                    TradeType = TradeType.BigBarPause
                };
            }
            return null;
        }

        public static TradeSetup? IsDojiConfirmedSetup(EquityPriceHistory prev, EquityPriceHistory curr)
        {
            if ((prev.Shape == CandleShape.Doji || prev.Shape == CandleShape.Hammer) &&
                prev.RangeZScore is >= -0.5 and <= 1.4 &&
                curr.Shape == CandleShape.FullBar && curr.RangeZScore is >= -1.05 and <= 1.4 && curr.Close > prev.Close)
            {
                return new TradeSetup
                {
                    PriceHistoryId = curr.Id,
                    TradeType = TradeType.DojiConfirmed
                };
            }
            return null;
        }

        public static TradeSetup? IsEngulfingTradeSetup(EquityPriceHistory prev, EquityPriceHistory curr)
        {
            if (prev.Shape != CandleShape.Hammer && prev.Shape != CandleShape.Doji && prev.RangeZScore is >= -1.05 and <= 1.4 
                && prev.Close < prev.Open &&
                (curr.Shape == CandleShape.FullBar || curr.Shape == CandleShape.TailBar) 
                && curr.RangeZScore is >= -1.05 and <= 1.4 && curr.Close > curr.Open)
            {
                decimal halfwayMark = (prev.Open - prev.Close) / 2;

                // Ensure current close is above the halfway mark
                if (curr.Close > prev.Close + halfwayMark)
                {
                    return new TradeSetup
                    {
                        PriceHistoryId = curr.Id,
                        TradeType = TradeType.Engulfing
                    };
                }
            }
            return null;
        }

        public static TradeSetup? IsContainedTailBarSetup(EquityPriceHistory prev, EquityPriceHistory curr)
        {
            if (prev.Shape == CandleShape.TailBar || prev.Shape == CandleShape.FullBar && prev.RangeZScore is >= 1.0 and <= 1.5 && prev.Close < prev.Open &&
                curr.RangeZScore is >= -1.05 and <= 1.4 &&
                curr.Open < prev.Close && curr.Close >= prev.Low)
            {
                return new TradeSetup
                {
                    PriceHistoryId = curr.Id,
                    TradeType = TradeType.ContainedTailBar
                };
            }
            return null;
        }

        public static TradeSetup? IsThreeTailsSetup(List<EquityPriceHistory> relevantHistory)
        {
            if (relevantHistory.Count >= 3)
            {
                var lastItem = relevantHistory.Last();
                if (isValidThreeTailsShape(lastItem))
                {
                    var priorTailBars = relevantHistory
                        .Take(relevantHistory.Count - 1)
                        .Where(isValidThreeTailsShape)
                        .ToList();
                    if (priorTailBars.Count >= 2)
                    {
                        EquityPriceHistory? firstHigherLow = null;
                        EquityPriceHistory? secondHigherLow = null;

                        for (int i = priorTailBars.Count - 1; i >= 0; i--)
                        {
                            var current = priorTailBars[i];

                            if (firstHigherLow == null && current.Low > lastItem.Low)
                            {
                                firstHigherLow = current;
                            }
                            else if (firstHigherLow != null && secondHigherLow == null && current.Low > firstHigherLow.Low)
                            {
                                secondHigherLow = current;
                                break; // No need to continue once both are found
                            }
                        }
                        if (firstHigherLow != null && secondHigherLow != null)
                        {
                            return new TradeSetup
                            {
                                PriceHistoryId = lastItem.Id,
                                TradeType = TradeType.ThreeTails
                            };
                        }
                    }
                }
            }
            return null;
        }

        public static async Task<TradeSetup?> Analyze(List<EquityPriceHistory>? relevantHistory, WavePoint? lastPeak) 
        {
            if ( relevantHistory == null || relevantHistory.Count<2)
            {
                return null;
            }

            if (lastPeak != null && lastPeak.Price < relevantHistory[1].Close * 1.03m)
            {
                return null;
            }

            // Create tasks for each check
            var tasks = new List<Task<TradeSetup?>>()
            {
                Task.Run(() => IsBigBarPauseTradeSetup(relevantHistory[0], relevantHistory[1])),
                Task.Run(() => IsEngulfingTradeSetup(relevantHistory[0], relevantHistory[1])),
                Task.Run(() => IsDojiConfirmedSetup(relevantHistory[0], relevantHistory[1])),
                Task.Run(() => IsContainedTailBarSetup(relevantHistory[0], relevantHistory[1])),
                Task.Run(() => IsThreeTailsSetup(relevantHistory))
            };

            // Wait for the first completed task that returns a non-null result
            var results = await Task.WhenAll(tasks);

            // Return the result of the first task that returns a non-null TradeSetup
            return results.FirstOrDefault(x => x != null);
        }
    }
}
