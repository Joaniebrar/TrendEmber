

namespace TrendEmber.Core.Trends
{
    public class ResistancePoints
    {
        public string Symbol { get; set; }
        public decimal? Resistance { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
