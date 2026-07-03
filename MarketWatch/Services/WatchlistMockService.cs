using MarketWatch.Models;

namespace MarketWatch.Services
{
    public class WatchlistMockService : IWatchlistService
    {
        private readonly List<Watchlist> _watchlists = new();
        private int _nextId = 1;

        public async Task<Watchlist?> GetWatchlistAsync(int id)
        {
            var watchlist = _watchlists.FirstOrDefault(w => w.Id == id);
            return await Task.FromResult(watchlist);
        }

        public async Task<Watchlist> CreateWatchlistAsync(List<string> tickers)
        {
            var newWatchlist = new Watchlist
            {
                Id = _nextId++,
                Tickers = tickers ?? new List<string>()
            };

            _watchlists.Add(newWatchlist);
            return await Task.FromResult(newWatchlist);
        }

        public async Task<bool> DeleteWatchlistAsync(int id)
        {
            var watchlist = _watchlists.FirstOrDefault(w => w.Id == id);
            if (watchlist == null)
            {
                return await Task.FromResult(false);
            }

            _watchlists.Remove(watchlist);
            return await Task.FromResult(true);
        }
    }
}