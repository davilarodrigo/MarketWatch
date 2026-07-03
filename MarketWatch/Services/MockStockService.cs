using MarketWatch.Models;

namespace MarketWatch.Services
{
    public class MockStockService : IStockService
    {
        private readonly List<Stock> _mockStocks = new()
        {
            new Stock { Ticker = "AAPL", CompanyName = "Apple Inc.", CurrentPrice = 300.50m, PriceChange = 1.20m, PercentChange = 0.69m, Volume = 52000000, MarketCap = 2750000000000m, LastUpdated = DateTime.UtcNow },
            new Stock { Ticker = "MSFT", CompanyName = "Microsoft Corporation", CurrentPrice = 385.25m, PriceChange = -3.40m, PercentChange = -0.80m, Volume = 23000000, MarketCap = 3120000000000m, LastUpdated = DateTime.UtcNow },
            new Stock { Ticker = "TSLA", CompanyName = "Tesla, Inc.", CurrentPrice = 420.00m, PriceChange = 5.75m, PercentChange = 3.40m, Volume = 88000000, MarketCap = 55000000000m, LastUpdated = DateTime.UtcNow },
            new Stock { Ticker = "NVDA", CompanyName = "NVIDIA Corporation", CurrentPrice = 210.00m, PriceChange = 12.30m, PercentChange = 1.38m, Volume = 45000000, MarketCap = 2200000000000m, LastUpdated = DateTime.UtcNow }
        };

        public async Task<Stock> GetStockByTickerAsync(string ticker)
        {
            var mockStocks= _mockStocks.FirstOrDefault(s => s.Ticker.Equals(ticker, StringComparison.OrdinalIgnoreCase));
            return await Task.FromResult(mockStocks);
        }
    }
}