using Microsoft.AspNetCore.Mvc;
using TrendEmber.Service;
using TrendEmber.Contract;
using TrendEmber.Contract.Mappers;

namespace TrendEmber_Solution.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TradeSetsController : ControllerBase
    {
        private ITradeService _tradeService;
        public TradeSetsController(ITradeService tradeService)
        {
            _tradeService = tradeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetTradeSetsAsync([FromQuery] string? cursor = null, [FromQuery] int pageSize = 10)
        {
            var result = await _tradeService.GetTradeSetsAsync(cursor, pageSize);
            return Ok(new CursorPagedResponse<TradeSetDto>(result.Item1.Select(TradeSetMapper.Map), result.nextCursor));
        }
    }
}
