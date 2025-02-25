using Microsoft.AspNetCore.Mvc;
using TrendEmber.Service;
using TrendEmber.Contract;
using TrendEmber.Contract.Mappers;

namespace TrendEmber_Solution.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WatchListsController : ControllerBase
    {
        private ITradeService _tradeService;
        public WatchListsController(ITradeService tradeService)
        {
            _tradeService = tradeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetWatchListsAsync([FromQuery] string? cursor = null, [FromQuery] int pageSize = 10)
        {
            var result = await _tradeService.GetWatchListsAsync(cursor, pageSize);
            return Ok(new CursorPagedResponse<WatchListDto>(result.Item1.Select(WatchListMapper.Map), result.nextCursor));
        }
        [HttpPost]
        public async Task<IActionResult> ImportWatchListAsync(
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
            var result = await _tradeService.ImportWatchListAsync(csvContent, name, mapping, ignoreFirstRow);

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
