using Microsoft.AspNetCore.Mvc;
using TrendEmber.Service;
using TrendEmber.Contract.Mappers;

namespace TrendEmber_Solution.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WatchListSymbolsController : Controller
    {
        private ITradeService _tradeService;

        public WatchListSymbolsController(ITradeService tradeService)
        {
            _tradeService = tradeService;
        }
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] Guid watchListId)
        {
            var result = await _tradeService.GetWatchListSymbolsAsync(watchListId);
            return Ok(result.Select(WatchListSymbolMapper.Map));
        }
    }
}
