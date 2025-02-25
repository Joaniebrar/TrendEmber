using System.ComponentModel.DataAnnotations;

namespace TrendEmber.Contract
{
    public class WatchListDto
    {
        public Guid Id { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string ImportedDate { get; set; } = string.Empty;
        [Range(0, int.MaxValue)]
        public int SymbolCount { get; set; }
    }
}
