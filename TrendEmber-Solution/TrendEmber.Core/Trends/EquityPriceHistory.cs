
namespace TrendEmber.Core.Trends
{
    public class EquityPriceHistory
    {
        public Guid Id { get; set; }
        public string Symbol { get; set; }
        public decimal Volume { get; set; }
        public decimal VolumeWeighted { get; set; } // VWAP (Volume Weighted Average Price)
        public decimal Open { get; set; }  // Open price
        public decimal Close { get; set; }  // Close price
        public decimal High { get; set; }  // High price
        public decimal Low { get; set; }  // Low price
        public DateTime PriceDate { get; set; }
        public decimal RawPriceDatee { get; set; }
        public ChartTime ChartTime { get; set; }
        public CandleShape? Shape { get; set; }    

        public double? RangeZScore { get; set; }

    }
}
