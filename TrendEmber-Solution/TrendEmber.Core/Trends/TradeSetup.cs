using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrendEmber.Core.Trends
{
    public enum TradeType { 
        BigBarPause,
        Engulfing,
        DojiConfirmed,
        ThreeTails,
        ContainedTailBar
    }
    public class TradeSetup
    {
        public Guid Id { get; set; }
        public Guid PriceHistoryId { get; set; }
        public EquityPriceHistory PriceHistory { get; set; }
    }
}
