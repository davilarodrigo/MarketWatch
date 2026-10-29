using MarketWatch.Interfaces;
using MarketWatch.Models;
using Microsoft.AspNetCore.Mvc;

namespace MarketWatch.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StockController : ControllerBase
    {
        private readonly IStockService _stockService;

        public StockController(IStockService stockService)
        {
            _stockService = stockService;
        }

        [HttpGet("{ticker}")]
        public async Task<ActionResult<Stock>> GetStock(string ticker)
        {
            if (string.IsNullOrWhiteSpace(ticker))
            {
                return BadRequest("Ticker symbol cannot be empty.");
            }

            var stock = await _stockService.GetStockByTickerAsync(ticker);

            if (stock == null)
            {
                return NotFound(new { message = $"Stock with ticker '{ticker.ToUpper()}' not found" });
            }

            return Ok(stock);
        }
    }
}