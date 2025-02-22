namespace TrendEmber.Core.Trends

{
    public class Trade
    {
        public Guid Id { get; set; }
        public Guid TradeSetId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Symbol { get; set; } = string.Empty;
        public ChartTime BasedOn { get; set; }
        public decimal Entry { get; set; }
        public decimal TG1 { get; set; }
        public decimal TG2 { get; set; }
        public decimal StopLoss { get; set; }

        public TradeSet? TradeSet { get; set; }

    }
}
