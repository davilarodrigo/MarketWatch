using MarketWatch.Models;
using MarketWatch.Services;
using Microsoft.AspNetCore.Mvc;

namespace MarketWatch.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WatchlistController : ControllerBase
    {
        private readonly IWatchlistService _watchlistService;

        public WatchlistController(IWatchlistService watchlistService)
        {
            _watchlistService = watchlistService;
        }

        // GET: api/watchlist/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Watchlist>> Get(int id)
        {
            var watchlist = await _watchlistService.GetWatchlistAsync(id);

            if (watchlist == null)
            {
                return NotFound(new { message = $"Watchlist with ID {id} not found." });
            }

            return Ok(watchlist);
        }

        // POST: api/watchlist
        // example ["AAPL", "MSFT"]
        [HttpPost]
        public async Task<ActionResult<Watchlist>> Post([FromBody] List<string> tickers)
        {
            var newWatchlist = await _watchlistService.CreateWatchlistAsync(tickers);

            // returns 201 (Created) and route to new resource
            return CreatedAtAction(nameof(Get), new { id = newWatchlist.Id }, newWatchlist);
        }

        // DELETE: api/watchlist/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var deleted = await _watchlistService.DeleteWatchlistAsync(id);

            if (!deleted)
            {
                return NotFound(new { message = $"Watchlist with ID {id} not found." });
            }

            return NoContent(); // 204 No Content (successful delete)
        }
    }
}