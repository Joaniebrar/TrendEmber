
namespace TrendEmber.Core.Trends
{
    public class TradeSetupSimulation
    {
        public Guid Id { get; set; }
        public Guid TradeSetupId { get; set; }

        public string Version { get; set; }
        public string Symbol { get; set; }
        public DateTime TradeDate { get; set; }
        public decimal? FirstResistance { get; set; }
        public decimal? SecondResistance { get; set; }
        public decimal? FirstSupport { get; set; }
        public decimal? SecondSupport { get; set; }
        public decimal? Entry { get; set; }
        public decimal? TG1 { get; set; }
        public decimal? TG2 { get; set; }
        public decimal? SL { get; set; }
        public decimal? TG1Percentage { get; set; }
        public decimal? TG2Percentage { get; set; }
        public decimal? SLPercentage { get; set; }
        public decimal? Exit { get; set; }
        public decimal? ExitPercentage { get; set; }
        public DateTime ExitDate { get; set; }

        public TradeSetup TradeSetup { get; set; }
    }
}
