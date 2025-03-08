using FluentAssertions;
using TrendEmber.Core;

namespace TrendEmber.Service.Tests
{
    public class CandleStickAnalyzerTests
    {


        [TestCase(21.02,20.13,20.95,20.66,false)]
        [TestCase(18.53, 17.34, 17.79, 18.01, true)]
        [TestCase(19.375, 18.59, 19.23, 19.19, true)]
        [TestCase(19.23, 18.12, 19.13, 18.81, false)]
        [TestCase(18.81, 17.82, 18.68, 18.27, false)]
        [TestCase(18.84, 18.02, 18.44, 18.32, true)]
        [TestCase(20.42, 18.02, 18.44, 18.32, false)]
        public void IsDoji_ValidInputs_ShouldReturnExpectedResult(decimal high, decimal low, decimal open, decimal close, bool expectedResult)
        {
            CandleStickAnalyzer.IsDoji(open,close,high,low).Should().Be(expectedResult);
        }

        [TestCase(21.02, 20.13, 20.95, 20.66, true)]
        [TestCase(19.375, 18.59, 19.23, 19.19, true)]
        [TestCase(19.23, 18.12, 19.13, 18.81, true)]
        [TestCase(18.81, 17.82, 18.68, 18.27, true)]
        public void IsTailBar_ValidInputs_ShouldReturnExpectedResult(decimal high, decimal low, decimal open, decimal close, bool expectedResult)
        {
            CandleStickAnalyzer.IsTailBar(open, close, high, low).Should().Be(expectedResult);
        }
        [TestCase(21.02, 20.13, 20.95, 20.66, false)]
        [TestCase(19.375, 18.59, 19.23, 19.19, false)]
        public void IsHammerBar_ValidInputs_ShouldReturnExpectedResult(decimal high, decimal low, decimal open, decimal close, bool expectedResult)
        {
            CandleStickAnalyzer.IsHammer(open, close, high, low).Should().Be(expectedResult);
        }
        [TestCase(21.02, 20.13, 20.95, 20.66, false)]
        [TestCase(19.55, 17.92, 18.2, 19.53, true)]
        [TestCase(19.93, 18.89, 19.55, 19.07, false)]
        [TestCase(19.27, 18.72, 18.75, 19.18, true)]
        [TestCase(19.375, 18.59, 19.23, 19.19, false)]
        [TestCase(18.845, 17.96, 18.13, 18.76, true)]
        [TestCase(19.51, 18.67, 18.68, 19.4, true)]
        [TestCase(19.43, 18.36, 19.4, 18.42, true)]
        [TestCase(19.96, 18.39, 18.41, 19.72, true)]
        [TestCase(17.02, 16.15, 16.32, 16.79, true)]
        [TestCase(17.905, 17.07, 17.25, 17.8, true)]
        public void IsFullBar_ValidInputs_ShouldReturnExpectedResult(decimal high, decimal low, decimal open, decimal close, bool expectedResult)
        {
            CandleStickAnalyzer.IsFullBar(open, close, high, low).Should().Be(expectedResult);
        }
        [TestCase(21.02, 20.13, 20.95, 20.66, CandleShape.TailBar)]
        [TestCase(19.55, 17.92, 18.2, 19.53, CandleShape.FullBar)]
        [TestCase(19.375, 18.59, 19.23, 19.19, CandleShape.Doji)]
        [TestCase(17.02, 16.15, 16.32, 16.79, CandleShape.FullBar)]
        [TestCase(17.905, 17.07, 17.25, 17.8, CandleShape.FullBar)]
        public void CalculateCandleShape_ValidInputs_ShouldReturnExpectedResult(decimal high, decimal low, decimal open, decimal close, CandleShape expectedResult)
        {
            CandleStickAnalyzer.CalculateCandleShape(open, close, high, low).Should().Be(expectedResult);
        }

        [Test]
        public void CalculateZScore_ValidInputs_ShouldReturnExpectedResults()
        {
            var result = CandleStickAnalyzer.CalculateZScore(7.61m, 7.25m, 0.6030235849056604, 0.2725400547165211);
            result.Should().Be(-0.8916985987928937);
        }
    }
}