using FluentAssertions;
using TrendEmber.Core.Trends;
using TrendEmber.Core;

namespace TrendEmber.Service.Tests
{
    [TestFixture]
    public class TradeSetupAnalyzerTests
    {
        /*
 * 
select sy."Symbol",wp."Price",wp."PriceDate" from public."WavePoints" wp
inner join public."Symbols" sy on wp."SymbolId" = sy."Id"
where sy."Symbol" = 'CCL'
and "PriceDate" <= '2024-04-22 22:00:00-06' and "Type"=0
order by "PriceDate" desc

SELECT "PriceDate",
'new EquityPriceHistory() {' ||
' Symbol = "' || "Symbol" || '", ' ||
' Open = ' || "Open" || 'm, ' ||
' Close = ' || "Close" || 'm, ' ||
' High = ' || "High" || 'm, ' ||
' Low = ' || "Low" || 'm, ' ||
' RangeZScore = ' || "RangeZScore" || ', ' ||
' PriceDate = DateTimeOffset.Parse("' || "PriceDate" || '").DateTime, ' ||
' Shape = CandleShape.' || "Shape" || ' ' ||
'},'
FROM public."EquityPrices"
WHERE "Symbol" = 'CCL'
AND "PriceDate" <= '2024-04-22 22:00:00-06'
ORDER BY "PriceDate" DESC;

 * */
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

        [Test]
        public async Task AnalyzeSeptember07202024SNOW_WithValidInputs_ShouldReturnTradeSetup()
        {
            var relevantHistory = new List<EquityPriceHistory>() {
                new EquityPriceHistory() { Symbol = "SNOW",  
                    Open = 113.06m,  Close = 108.56m,  High = 114.73m,  Low = 107.13m,  
                    RangeZScore = -0.9224019477195047,  
                    PriceDate = DateTimeOffset.Parse("2024-08-31 22:00:00-06").DateTime,  Shape = CandleShape.FullBar },
                new EquityPriceHistory() { Symbol = "SNOW",  
                    Open = 109.32m,  Close = 113.67m,  High = 115.04m,  Low = 108.13m,  
                    RangeZScore = -1.0064650057006788,  
                    PriceDate = DateTimeOffset.Parse("2024-09-07 22:00:00-06").DateTime,  
                    Shape = CandleShape.FullBar },
            };
            var lastPeak = new WavePoint()
            {
                Price = 128.56m,
                PriceDate = DateTimeOffset.Parse("2024-08-17 22:00:00-06").DateTime
            };

            var result = await TradeSetupAnalyzer.Analyze(relevantHistory, lastPeak);
            result.Should().NotBeNull();
        }

        [Test]
        public async Task AnalyzeAugust3202024SIRI_WithValidInputs_ShouldReturnTradeSetup()
        {
            var relevantHistory = new List<EquityPriceHistory>() {
                new EquityPriceHistory() { Symbol = "SIRI",  
                    Open = 36.4m,  Close = 31.2m,  High = 36.9m,  Low = 30.65m,  
                    RangeZScore = 0.4570077974275946,  
                    PriceDate = DateTimeOffset.Parse("2024-07-27 22:00:00-06").DateTime,  
                    Shape = CandleShape.FullBar },
                new EquityPriceHistory() { Symbol = "SIRI",  
                    Open = 28.8m,  Close = 31.4m,  High = 32m,  Low = 28.6m,  
                    RangeZScore = -0.22999533257647725,  
                    PriceDate = DateTimeOffset.Parse("2024-08-03 22:00:00-06").DateTime,  
                    Shape = CandleShape.FullBar },
            };
            var lastPeak = new WavePoint()
            {
                Price = 37.2m,
                PriceDate = DateTimeOffset.Parse("2024-07-20 22:00:00-06").DateTime
            };

            var result = await TradeSetupAnalyzer.Analyze(relevantHistory, lastPeak);
            result.Should().BeNull();
        }

        [Test]
        public async Task AnalyzeSeptember28202024CSGP_WithValidInputs_ShouldReturnTradeSetup()
        {
            var relevantHistory = new List<EquityPriceHistory>() {
                new EquityPriceHistory() { Symbol = "CSGP",  
                    Open = 77.8m,  Close = 75.01m,  High = 78.27m,  Low = 73.59m,  
                    RangeZScore = 0.017807446920621695,  
                    PriceDate = DateTimeOffset.Parse("2024-09-21 22:00:00-06").DateTime,  
                    Shape = CandleShape.FullBar },
                new EquityPriceHistory() { Symbol = "CSGP",  
                    Open = 74.95m,  Close = 74.1m,  High = 75.67m,  Low = 73.15m,  
                    RangeZScore = -0.9376976542426743,  
                    PriceDate = DateTimeOffset.Parse("2024-09-28 22:00:00-06").DateTime,  
                    Shape = CandleShape.TailBar },
            };
            var lastPeak = new WavePoint()
            {
                Price = 79.86m,
                PriceDate = DateTimeOffset.Parse("2024-09-14 22:00:00-06").DateTime
            };

            var result = await TradeSetupAnalyzer.Analyze(relevantHistory, lastPeak);
            result.Should().BeNull();
        }

        [Test]
        public async Task AnalyzeAugust10202024EL_WithValidInputs_ShouldReturnTradeSetup()
        {
            var relevantHistory = new List<EquityPriceHistory>() {
                new EquityPriceHistory() { Symbol = "EL",  
                    Open = 91.17m,  Close = 89.15m,  High = 94.81m,  Low = 88.94m,  
                    RangeZScore = -0.6123470220645716,  
                    PriceDate = DateTimeOffset.Parse("2024-08-03 22:00:00-06").DateTime,  
                    Shape = CandleShape.Unknown },
                new EquityPriceHistory() { Symbol = "EL",  
                    Open = 89.15m,  Close = 94.97m,  High = 96.31m,  Low = 86.05m,  
                    RangeZScore = -0.005876299357472126,  
                    PriceDate = DateTimeOffset.Parse("2024-08-10 22:00:00-06").DateTime,  
                    Shape = CandleShape.FullBar },
            };
            var lastPeak = new WavePoint()
            {
                Price = 147.45m,
                PriceDate = DateTimeOffset.Parse("2024-04-20 22:00:00-06").DateTime
            };

            var result = await TradeSetupAnalyzer.Analyze(relevantHistory, lastPeak);
            result.Should().NotBeNull();
        }
    }
}
