﻿using TrendEmber.Core.Trends;

namespace TrendEmber.Service
{
    public interface ITradeService
    {
        public Task<(IEnumerable<TradeSet>, string? nextCursor)> GetTradeSetsAsync(string? cursor, int pageSize);
        public Task<Result<object>> ImportTradeSetsAsync(string? file, string name, string mapping, bool ignoreFirstRow);

        public Task<(IEnumerable<WatchList>, string? nextCursor)> GetWatchListsAsync(string? cursor, int pageSize);
        public Task<Result<object>> ImportWatchListAsync(string? file, string name, string mapping, bool ignoreFirstRow);

    }
}
