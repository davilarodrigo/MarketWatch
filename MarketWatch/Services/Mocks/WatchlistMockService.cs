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
            Random random = new Random();
            
            List<Stock> stocks=[];
            foreach(string ticker in tickers)
            {
                //mock stocks for testing purpouses only
                Stock stock = new Stock
                {
                    Ticker = ticker,
                    CurrentPrice = 100 + random.Next(100, 1000)
                };
                stocks.Add(stock);
                
            }

            var newWatchlist = new Watchlist
            {
                Id = _nextId++,
                Tickers = tickers ?? new List<string>(),
                Stocks =stocks
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