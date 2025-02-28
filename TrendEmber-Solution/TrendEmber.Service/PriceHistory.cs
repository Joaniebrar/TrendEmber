
namespace TrendEmber.Service
{
    public class PriceHistory
    {
        public decimal v { get; set; }  // Volume
        public decimal vw { get; set; } // VWAP (Volume Weighted Average Price)
        public decimal o { get; set; }  // Open price
        public decimal c { get; set; }  // Close price
        public decimal h { get; set; }  // High price
        public decimal l { get; set; }  // Low price
        public long t { get; set; }
    }
}
