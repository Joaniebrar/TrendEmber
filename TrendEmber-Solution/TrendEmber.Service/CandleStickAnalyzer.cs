
namespace TrendEmber.Service
{
    public static class CandleStickAnalyzer
    {
        public static bool IsDoji(decimal open, decimal close, decimal high, decimal low)
        {
            decimal range = (high - low) / 3;
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
            decimal range = (high - low) / 6;
            return Math.Max(open, close) >= high - range &&
                   Math.Min(open, close) <= low + range;
        }



    }
}
