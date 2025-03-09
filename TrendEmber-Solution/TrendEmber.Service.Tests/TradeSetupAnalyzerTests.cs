using FluentAssertions;
using TrendEmber.Core.Trends;
using TrendEmber.Core;

namespace TrendEmber.Service.Tests
{
    [TestFixture]
    public class TradeSetupAnalyzerTests
    {
        
        [Test]
        public async Task AnalyzeJuly62024BITO_WithValidInputs_ShouldReturnTradeSetup()
        {
            var relevantHistory = new List<EquityPriceHistory>() {
                new EquityPriceHistory(){
                    Symbol = "BITO",
                    Open = 22.1m,
                    Close = 19.76m,
                    High = 22.41m,
                    Low = 19.385m,
                    RangeZScore =1.1458423929305606,
                    PriceDate = DateTimeOffset.Parse("2024-06-29 22:00:00-06").DateTime,
                    Shape = CandleShape.FullBar
                },
                new EquityPriceHistory(){
                    Symbol = "BITO",
                    Open = 20.06m,
                    Close = 20.22m,
                    High = 20.675m,
                    Low = 19.26m,
                    RangeZScore =-0.4525865841070062,
                    PriceDate = DateTimeOffset.Parse("2024-06-29 22:00:00-06").DateTime,
                    Shape = CandleShape.Doji
                },
            };
            var lastPeak = new WavePoint()
            {
                Price = 28.07m,
                PriceDate = DateTimeOffset.Parse("2024-05-18 22:00:00-06").DateTime
            };

            var result = await TradeSetupAnalyzer.Analyze(relevantHistory, lastPeak);
            result.Should().NotBeNull(); 
        }

        [Test]
        public async Task AnalyzeMay132024INTC_WithValidInputs_ShouldReturnTradeSetup()
        {
            var relevantHistory = new List<EquityPriceHistory>() {
                new EquityPriceHistory(){
                    Symbol = "INTC",
                    Open = 31.115m,
                    Close = 29.85m,
                    High = 31.45m,
                    Low = 29.73m,
                    RangeZScore =-0.6873329491085486,
                    PriceDate = DateTimeOffset.Parse("2024-05-04 22:00:00-06").DateTime,
                    Shape = CandleShape.FullBar
                },
                new EquityPriceHistory(){
                    Symbol = "INTC",
                    Open = 30.03m,
                    Close = 31.83m,
                    High = 32.26m,
                    Low = 30m,
                    RangeZScore =-0.4525865841070062,
                    PriceDate = DateTimeOffset.Parse("2024-05-11 22:00:00-06").DateTime,
                    Shape = CandleShape.FullBar
                },
            };
            var lastPeak = new WavePoint()
            {
                Price = 44.29m,
                PriceDate = DateTimeOffset.Parse("2024-03-30 22:00:00-06").DateTime
            };

            var result = await TradeSetupAnalyzer.Analyze(relevantHistory, lastPeak);
            result.Should().NotBeNull();
        }

        [Test]
        public async Task AnalyzeApril202024INTC_WithValidInputs_ShouldReturnTradeSetup()
        {
            var relevantHistory = new List<EquityPriceHistory>() {
                new EquityPriceHistory() { Symbol = "CCL",  
                    Open = 14.66m,  Close = 14.12m,  High = 14.76m,  Low = 13.795m,  
                    RangeZScore = -0.8236557373823095,  
                    PriceDate = DateTimeOffset.Parse("2024-04-13 22:00:00-06").DateTime,  
                    Shape = CandleShape.FullBar },
                new EquityPriceHistory() { Symbol = "CCL",  
                    Open = 14.35m,  Close = 15.08m,  High = 15.225m,  Low = 14.07m,  
                    RangeZScore = -0.5535061127889728,  
                    PriceDate = DateTimeOffset.Parse("2024-04-20 22:00:00-06").DateTime,  
                    Shape = CandleShape.FullBar },
            };
            var lastPeak = new WavePoint()
            {
                Price = 17.34m,
                PriceDate = DateTimeOffset.Parse("2024-03-23 22:00:00-06").DateTime
            };

            var result = await TradeSetupAnalyzer.Analyze(relevantHistory, lastPeak);
            result.Should().NotBeNull();
        }

    }
}
