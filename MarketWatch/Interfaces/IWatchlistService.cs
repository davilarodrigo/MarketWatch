using MarketWatch.Models;
using ProductCatalogAPI.Common;

namespace MarketWatch.Interfaces
{
    public interface IWatchlistService
    {
        Task<Result<Watchlist?>> GetWatchlistAsync(int id);
        Task<Result<Watchlist>> CreateWatchlistAsync(List<string> tickers);
        Task<Result<bool>> DeleteWatchlistAsync(int id);
       
    }
}