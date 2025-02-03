using TrendEmber.Core.Trends;

namespace TrendEmber.Service
{
    public interface ITradeService
    {
        public Task<(IEnumerable<TradeSet>, string? nextCursor)> GetTradeSetsAsync(string? cursor, int pageSize);
    }
}
