
namespace TrendEmber.Core.Trends
{
    public enum GapDirection { GapUp, GapDown }
    public class PriceGapEvent
    {
        public Guid Id { get; set; }
        public Guid ClosingEquityPriceHistoryId { get; set; }
        public Guid OpeningEquityPriceHistoryId { get; set; }
        public GapDirection Direction { get; set; }
        public Guid? GapFilledPriceHistoryId { get; set; } // Nullable, only set when the gap is filled

        // Navigation properties for EF
        public EquityPriceHistory ClosingPriceHistory { get; set; }
        public EquityPriceHistory OpeningPriceHistory { get; set; }
        public EquityPriceHistory? GapFilledPriceHistory { get; set; }
    }
}
