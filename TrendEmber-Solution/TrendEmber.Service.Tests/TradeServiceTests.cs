using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrendEmber.Core.Trends;

namespace TrendEmber.Service.Tests
{
    public class TradeServiceTests
    {
        [Test]
        public void CalculateWavePrice_ForDojiTrough_ShouldReturnLow() {
            var candle = new EquityPriceHistory()
            {
                Open = 7.46m,
                Close = 7.52m,
                High = 7.61m,
                Low = 7.25m,
                RangeZScore = -0.8862206831947339
            };
            var waveType = WaveType.Trough;
            var result = TradeService.CalculateWavePrice(candle, waveType);
            result.Should().Be(candle.Low);
        }
    }
}
