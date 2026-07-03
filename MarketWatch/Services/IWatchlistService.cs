using MarketWatch.Models;

namespace MarketWatch.Services
{
    public interface IWatchlistService
    {
        Task<Watchlist?> GetWatchlistAsync(int id);
        Task<Watchlist> CreateWatchlistAsync(List<string> tickers);
        Task<bool> DeleteWatchlistAsync(int id);
    }
}