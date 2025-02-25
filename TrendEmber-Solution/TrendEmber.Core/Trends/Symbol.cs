
namespace TrendEmber.Core.Trends
{
    public class WatchListSymbol
    {
        public Guid Id { get; set; }
        public Guid WatchListId { get; set; }
        public string CompanyName { get; set; } = string.Empty;
        public string Market { get; set; } = string.Empty;
        public string Symbol { get; set; } = string.Empty;
        public WatchList WatchList { get; set; }
    }
}
