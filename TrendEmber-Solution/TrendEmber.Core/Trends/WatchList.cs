
namespace TrendEmber.Core.Trends
{
    public class WatchList
    {
        public WatchList(string name)
        {
            Name = name;
        }
        public Guid Id { get; set; }
        public Guid? HarvesterAgentId { get; set; }
        public DateTime ImportedDate { get; set; }
        public string Name { get; set; }
        public ICollection<WatchListSymbol> Symbols { get; set; } = new List<WatchListSymbol>();
        public HarvesterAgent Agent { get; set; }
    }
}
