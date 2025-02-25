
namespace TrendEmber.Core.Trends
{
    public class ApiProvider
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string BaseUrl { get; set; }
        public ICollection<HarvesterAgent> Agents { get; set; } = new List<HarvesterAgent>();
    }
}
