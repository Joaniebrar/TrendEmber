
using System;
using TrendEmber.Core;

namespace TrendEmber.Service
{
    public static class CandleStickAnalyzer
    {
        public static bool IsDoji(decimal open, decimal close, decimal high, decimal low)
        {
            decimal range = (high - low) / 4;
            decimal threshold = low + range;
            return open >= threshold && close >= threshold && Math.Abs(open - close) <= range;
        }

        public static bool IsHammer(decimal open, decimal close, decimal high, decimal low)
        {
            decimal range = (high - low) / 8;
            decimal threshold = high - (high - low) / 3;
            decimal maxOC = Math.Max(open, close);
            decimal minOC = Math.Min(open, close);

            return maxOC >= threshold &&
                   minOC >= threshold &&
                   (minOC <= threshold + range && maxOC >= high - range);
        }
        public static bool IsFullBar(decimal open, decimal close, decimal high, decimal low)
        {
            var grace = (high - low) / 2.15m;
            var top = Math.Max(open, close);
            var bottom = Math.Min(open, close);
            return grace >= (high - top) + (bottom - low);
        }

        public static bool IsTailBar(decimal open, decimal close, decimal high, decimal low)
        {
            var range = low + (high - low) / 2.75m;
            return open >= range && close >= range;
        }
        public static double CalculateZScore(decimal high, decimal low, double mean, double standardDeviation)
        {
            var range = (double)(high - low);
            return (range - mean) / standardDeviation;
        }

        public static CandleShape CalculateCandleShape(decimal open, decimal close, decimal high, decimal low) =>
            IsHammer(open, close, high, low) ? CandleShape.Hammer :
            IsDoji(open, close, high, low) ? CandleShape.Doji :
            IsTailBar(open, close, high, low) ? CandleShape.TailBar :
            IsFullBar(open, close, high, low) ? CandleShape.FullBar :
            CandleShape.Unknown;

    }



}
