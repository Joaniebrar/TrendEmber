using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrendEmber.Service
{
    public class MarketDay
    {
        public DateTime Date { get; set; }
        public double Open { get; set; }
        public double Close { get; set; }
        public double High { get; set; }
        public double Low { get; set; }

        public MarketDay(DateTime date, double open, double close, double high, double low)
        {
            Date = date;
            Open = open;
            Close = close;
            High = high;
            Low = low;
        }
    }
}
