
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
        public DateTime? LastImportedDate { get; set; }
        public double? MeanRange { get; set; }
        public double? StandardDeviation { get; set; }
        public ICollection<WavePoint> WavePoints { get; set; } = new List<WavePoint>();
    }
}
