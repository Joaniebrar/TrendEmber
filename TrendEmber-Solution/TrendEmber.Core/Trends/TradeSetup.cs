
namespace TrendEmber.Core.Trends
{
    public enum TradeType { 
        BigBarPause,
        Engulfing,
        DojiConfirmed,
        ThreeTails,
        ContainedTailBar
    }
    public class TradeSetup
    {
        public Guid Id { get; set; }
        public Guid PriceHistoryId { get; set; }

        public TradeType TradeType { get; set; }
        public EquityPriceHistory PriceHistory { get; set; }
        
    }
}
