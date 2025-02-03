namespace TrendEmber.Core.Trends
{
    public class TradeSet
    {
        public TradeSet(string name)
        {
            Name = name;
        }
        public Guid Id { get; set; }
        public DateTime ImportedDate { get; set; }
        public string Name { get; set; }
        public ICollection<Trade> Trades { get; set; } = new List<Trade>();
    }
}
