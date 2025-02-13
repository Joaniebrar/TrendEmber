using TrendEmber.Core.Trends;

namespace TrendEmber.Contract.Mappers
{
    public static class TradeSetMapper
    {
        public static TradeSetDto Map(TradeSet tradeSet) => new TradeSetDto{
            Id = tradeSet.Id,
            Name = tradeSet.Name,
            ImportedDate = tradeSet.ImportedDate.ToUniversalTime().ToString("o"),
            TradeCount = tradeSet.Trades.Count()
        };
    }
}
