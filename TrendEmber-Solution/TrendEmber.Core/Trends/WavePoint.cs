

namespace TrendEmber.Core.Trends
{
    public class WavePoint
    {
        public Guid Id { get; set; }
        public DateTime PriceDate { get; set; }  // The date of the wave point
        public decimal Price { get; set; }       // The price at the peak or trough
        public WaveType Type { get; set; }       // Identifies whether it's a peak or trough

        public Guid SymbolId { get; set; }
        public WatchListSymbol Symbol { get; set; }

        public Guid PriceHistoryId { get; set; }
        public EquityPriceHistory PriceHistory { get; set; }
        public override string ToString()
        {
            return $"{PriceDate:yyyy-MM-dd} - {Type}: {Price}";
        }
    }
}
