using MarketWatch.Models;
using ProductCatalogAPI.Common;

namespace MarketWatch.Interfaces
{
    public interface IStockService
    {
        Task<Result<Stock>> GetStockByTickerAsync(string ticker);
    }
}