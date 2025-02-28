using Microsoft.AspNetCore.Mvc;
using TrendEmber.Service;

namespace TrendEmber_Solution.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RunAgentController : Controller
    {
        private ITradeService _tradeService;
        public RunAgentController(ITradeService tradeService)
        {
            _tradeService = tradeService;
        }

        [HttpPost]
        public async Task<IActionResult> RunAgentAsync([FromBody] RunAgentRequest request)
        {
            await _tradeService.RunAgentForWatchlist(request.WatchList);            
            return Ok("US Buddy");
        }
    }

    public class RunAgentRequest
    {
        public Guid WatchList { get; set; }
    }
}
