using TrendEmber.Core.Trends;

namespace TrendEmber.Service
{
    public interface ITradeService 
    {
        public Task<(IEnumerable<TradeSet>, string? nextCursor)> GetTradeSetsAsync(string? cursor, int pageSize);
        public Task<Result<object>> ImportTradeSetsAsync(string? file, string name, string mapping, bool ignoreFirstRow);

        public Task<(IEnumerable<WatchList>, string? nextCursor)> GetWatchListsAsync(string? cursor, int pageSize);
        public Task<Result<object>> ImportWatchListAsync(string? file, string name, string mapping, bool ignoreFirstRow);
        public Task<IEnumerable<WatchListSymbol>> GetWatchListSymbolsAsync(Guid watchListId);
        public Task RunAgentForWatchlist(Guid watchListId);
        public void CalculateMeanAndStandardDeviation();
        public void CalculatePriceHistoryShapeZScore();
        public Task FindPeaksAndTroughsForWatchListAsync();
        public Task DetectGapsAsync();
        public Task DetectTradeSetups();
        public Task SimulateTrades();
        public Task CalculateSimulationResults();
        public Task CalculateExits();

        public Task RunWeeklyImport();
    }
}
