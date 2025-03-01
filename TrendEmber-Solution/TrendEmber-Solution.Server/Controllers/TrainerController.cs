using Microsoft.AspNetCore.Mvc;
using TrendEmber.Service;
using TrendEmber.Contract;

namespace TrendEmber_Solution.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TrainerController : Controller
    {
        private ITradeService _tradeService;

        public TrainerController(ITradeService tradeService)
        {
            _tradeService = tradeService;
        }
        [HttpGet]
        public async Task<IActionResult> GetCandleAsync()
        {

            Random rand = new Random();
            decimal start = 50m;
            decimal max = 55m;
            decimal min = 45m;
            var high = (decimal)(rand.NextDouble() * (double)(max - start) + (double)start);
            var low = (decimal)(rand.NextDouble() * (double)(start - min) + (double)min);
            decimal close = (decimal)(rand.NextDouble() * (double)(Math.Abs(high - low))) + Math.Min(high, low);


            var result = new TrainerDto()
            {
                Open = start,
                High = high,
                Low = low,
                Close = close,
                Doji = CandleStickAnalyzer.IsDoji(start, close, high, low),
                FullBar = CandleStickAnalyzer.IsFullBar(start, close, high, low),
                Hammer = CandleStickAnalyzer.IsHammer(start, close, high, low),
            };
            return Ok(result);
        }
    }
}
