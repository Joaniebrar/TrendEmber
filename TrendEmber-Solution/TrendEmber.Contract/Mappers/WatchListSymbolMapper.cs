using TrendEmber.Core.Trends;

namespace TrendEmber.Contract.Mappers
{
    public class WatchListSymbolMapper
    {
        public static WatchListSymbolDto Map(WatchListSymbol symbol) => new WatchListSymbolDto
        {
            
            Market = symbol.Market,                        
            Name = symbol.CompanyName,
            Symbol = symbol.Symbol,
            LastRecordedPrice = 0.00m //todo
        };
    }
}
