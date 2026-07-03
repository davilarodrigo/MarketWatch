using MarketWatch.Models;

namespace MarketWatch.Services
{
    public interface IStockService
    {
         Task<Stock> GetStockByTickerAsync(string ticker);
    }
}