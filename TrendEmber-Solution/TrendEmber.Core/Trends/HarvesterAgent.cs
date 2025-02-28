
namespace TrendEmber.Core.Trends
{
    public class HarvesterAgent
    {
        public Guid Id { get; set; }
        public Guid ApiProviderId { get; set; }
        public ApiProvider ApiProvider { get; set; }
        public WatchList WatchList { get; set; }
        public HarvesterStatus Status { get; set; }

    }
}
