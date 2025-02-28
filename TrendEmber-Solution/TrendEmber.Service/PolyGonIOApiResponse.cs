

namespace TrendEmber.Service
{
    public class PolyGonIOApiResponse
    {
        public int queryCount { get; set; }
        public int resultsCount { get; set; }
        public List<PriceHistory> results { get; set; }
    }
}
