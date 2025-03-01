
namespace TrendEmber.Contract
{
    public class TrainerDto
    {
        public bool Doji { get; set; }
        public bool FullBar { get; set; }
        public bool Hammer { get; set; }
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }
    }
}
