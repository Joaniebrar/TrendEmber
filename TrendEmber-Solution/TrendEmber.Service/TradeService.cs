using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrendEmber.Core.Trends;
using TrendEmber.Data;

namespace TrendEmber.Service
{
    public class TradeService
    {
        private TrendsDbContext _dbContext;
        public TradeService(TrendsDbContext trendsDbContext) { 
            _dbContext = trendsDbContext;
        }
        public async Task<(IEnumerable<TradeSet>, string? nextCursor)> GetTradeSetsAsync(string? cursor, int pageSize) 
        {
            var query = _dbContext.TradeSets.OrderByDescending(ts => ts.ImportedDate).AsQueryable();

            if (!string.IsNullOrEmpty(cursor) && DateTime.TryParse(cursor, out var lastDate))
            {
                query = query.Where(ts => ts.ImportedDate < lastDate);
            }

            var tradeSets =  await query.Take(pageSize).ToListAsync();
            var nextCursor = tradeSets.Any() ? tradeSets.Last().ImportedDate.ToString("O") : null;
            return (tradeSets, nextCursor);
        }
    }
}
