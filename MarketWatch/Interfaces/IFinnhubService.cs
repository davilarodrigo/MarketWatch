using MarketWatch.Models;
using ProductCatalogAPI.Common;

namespace MarketWatch.Interfaces
{
    public interface IFinnhubService
    {
        Task<Result<Stock>> GetStockFromApiAsync(string ticker);
    }
}