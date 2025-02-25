
using TrendEmber.Core.Trends;

namespace TrendEmber.Contract.Mappers
{
    public static class WatchListMapper
    {
        public static WatchListDto Map(WatchList watchList) => new WatchListDto
        {
            Id = watchList.Id,
            Name = watchList.Name,
            ImportedDate = watchList.ImportedDate.ToUniversalTime().ToString("o"),
            SymbolCount = watchList.Symbols.Count()
        };
    }
}
