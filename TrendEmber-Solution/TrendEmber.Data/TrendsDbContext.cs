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

        public DbSet<HarvesterAgent> Agents { get; set; }
        public DbSet<ApiProvider> ApiProviders { get; set; }
        public DbSet<EquityPriceHistory> EquityPrices { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Trade>()
                .HasOne(t => t.TradeSet)
                .WithMany(ts => ts.Trades)
                .HasForeignKey(t => t.TradeSetId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<WatchListSymbol>()
                .HasOne(wls => wls.WatchList)
                .WithMany(wl => wl.Symbols)
                .HasForeignKey(wls => wls.WatchListId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<WatchList>()
                .HasOne(w => w.Agent)
                .WithOne(a => a.WatchList)
                .HasForeignKey<WatchList>(w => w.HarvesterAgentId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<HarvesterAgent>()
                .HasOne(ha => ha.ApiProvider)
                .WithMany(ap => ap.Agents)
                .HasForeignKey(ha => ha.ApiProviderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<EquityPriceHistory>()
                .HasIndex(e => new { e.Symbol, e.PriceDate })
                .HasDatabaseName("IX_Symbol_PriceDate");
        }
      
    }
}
