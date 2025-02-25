using Microsoft.EntityFrameworkCore;
using TrendEmber.Core.Trends;

namespace TrendEmber.Data
{
    public class TrendsDbContext : DbContext
    {
        public TrendsDbContext(DbContextOptions<TrendsDbContext> options)
    : base(options)
        {
        }

        public DbSet<TradeSet> TradeSets { get; set; }
        public DbSet<Trade> Trades { get; set; }

        public DbSet<WatchList> WatchList { get; set; }
        public DbSet<WatchListSymbol> Symbols { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Trade>()
                .HasOne(t => t.TradeSet)
                .WithMany(ts => ts.Trades)
                .HasForeignKey(t => t.TradeSetId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
