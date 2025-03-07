using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public static TradeSetup? Analyze(List<EquityPriceHistory>? relevantHistory) 
        {
            if ( relevantHistory == null || relevantHistory.Count<2)
            {
                return null;
            }
            var prev = relevantHistory[0];
            var curr = relevantHistory[1];

            // Setup 1: FullBar with Z-Score between 1.5 and 5, followed by Hammer or Doji
            if (prev.Shape == CandleShape.FullBar && prev.RangeZScore is >= 1.5 and <= 5 &&
                prev.Open > prev.Close &&
                (curr.Shape == CandleShape.Hammer || curr.Shape == CandleShape.Doji))
            {
                return new TradeSetup
                {
                    Id = Guid.NewGuid(),
                    PriceHistoryId = curr.Id,
                    PriceHistory = curr,
                    TradeType = TradeType.BigBarPause
                };
            }

            // Setup 2: FullBar with Z-Score between -0.5 and 1.4, prev closes lower, curr closes higher
            if (prev.Shape == CandleShape.FullBar && prev.RangeZScore is >= -0.5 and <= 1.4 && prev.Close < prev.Open &&
                (curr.Shape == CandleShape.FullBar || curr.Shape == CandleShape.TailBar) && curr.RangeZScore is >= -0.5 and <= 1.4 && curr.Close > curr.Open)
            {
                return new TradeSetup
                {
                    Id = Guid.NewGuid(),
                    PriceHistoryId = curr.Id,
                    PriceHistory = curr,
                    TradeType = TradeType.Engulfing
                };
            }

            // Setup 3: Doji or Hammer followed by FullBar closing higher
            if ((prev.Shape == CandleShape.Doji || prev.Shape == CandleShape.Hammer) &&
                prev.RangeZScore is >= -0.5 and <= 1.4 &&
                curr.Shape == CandleShape.FullBar && curr.RangeZScore is >= -0.5 and <= 1.4 && curr.Close > prev.Close)
            {
                return new TradeSetup
                {
                    Id = Guid.NewGuid(),
                    PriceHistoryId = curr.Id,
                    PriceHistory = curr,
                    TradeType = TradeType.DojiConfirmed
                };
            }

            // Setup 4: TailBar with Z-Score 1-1.5 and closing lower, followed by FullBar
            if (prev.Shape == CandleShape.TailBar && prev.RangeZScore is >= 1.0 and <= 1.5 && prev.Close < prev.Open &&
                curr.Shape == CandleShape.FullBar && curr.RangeZScore is >= -0.5 and <= 1.4 &&
                curr.Open < prev.Close && curr.Close >= prev.Low)
            {
                return new TradeSetup
                {
                    Id = Guid.NewGuid(),
                    PriceHistoryId = curr.Id,
                    PriceHistory = curr,
                    TradeType = TradeType.ContainedTailBar
                };
            }


            // Setup 5: Three TailBars, Dojis, or Hammers between two wavepoints
            if (relevantHistory.Count >= 3)
            {
                var lastItem = relevantHistory.Last();
                if (isValidThreeTailsShape(lastItem)) {
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
                                Id = Guid.NewGuid(),
                                PriceHistoryId = lastItem.Id,
                                PriceHistory = lastItem,
                                TradeType = TradeType.ThreeTails
                            };
                        }
                    }
                }
               
            }
            
            return null;
        }
    }
}
