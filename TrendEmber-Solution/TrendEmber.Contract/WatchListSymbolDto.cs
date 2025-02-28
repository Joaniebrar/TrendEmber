using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrendEmber.Contract
{
    public class WatchListSymbolDto
    {
        public string Market { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }

        public decimal LastRecordedPrice { get; set; }
    }
}
