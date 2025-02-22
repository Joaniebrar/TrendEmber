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
        [HttpPost]
        public async Task<IActionResult> ImportTradeSetAsync(
            [FromForm] IFormFile file,
            [FromForm] string name,
            [FromForm] string mapping,
            [FromForm] bool ignoreFirstRow)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("CSV file is required.");
            }

            using var stream = new StreamReader(file.OpenReadStream());
            var csvContent = await stream.ReadToEndAsync();
            var result = await _tradeService.ImportTradeSetsAsync(csvContent, name, mapping, ignoreFirstRow);

            if (result.Success)
            {
                return Ok(new { message = "File uploaded successfully", name, mapping, ignoreFirstRow });
            }
            else
            {
                return BadRequest(new { message = result.Message ?? "File upload failed", details = result.Data });
            }
        }
    }
}
