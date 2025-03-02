

namespace TrendEmber.Service
{
    public class MarketOutlier
    {
        private List<MarketDay> _marketDays;

        // Constructor to initialize with a list of MarketDays
        public MarketOutlier(List<MarketDay> marketDays)
        {
            _marketDays = marketDays;
        }

        // Method to calculate the mean of the Close prices
        public double CalculateMean()
        {
            if (_marketDays.Count == 0)
                throw new InvalidOperationException("No market days available.");

            return _marketDays.Average(m => m.Close);
        }

        // Method to calculate the standard deviation of the Close prices
        public double CalculateStandardDeviation()
        {
            if (_marketDays.Count == 0)
                throw new InvalidOperationException("No market days available.");

            double mean = CalculateMean();
            double variance = _marketDays.Average(m => Math.Pow(m.Close - mean, 2));
            return Math.Sqrt(variance);
        }

        // Method to calculate days larger than normal but not extreme (1-2 standard deviations above mean)
        public List<MarketDay> GetLargerThanNormalButNotExtreme()
        {
            double mean = CalculateMean();
            double standardDeviation = CalculateStandardDeviation();

            double lowerThreshold = mean + standardDeviation;   // 1 SD above mean
            double upperThreshold = mean + 2 * standardDeviation; // 2 SD above mean

            return _marketDays.Where(m => m.Close > lowerThreshold && m.Close <= upperThreshold).ToList();
        }
    }
}