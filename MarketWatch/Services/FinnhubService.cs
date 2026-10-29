using MarketWatch.Models;
using MarketWatch.Data;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using MarketWatch.Interfaces;
using ProductCatalogAPI.Common;

namespace MarketWatch.Services
{
    public class FinnhubService : IFinnhubService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly ApplicationDbContext _context; // Add EF Core context

        // add DbContext 
        public FinnhubService(HttpClient httpClient, IConfiguration configuration, ApplicationDbContext context)
        {
            _httpClient = httpClient;
            _apiKey = configuration["Finnhub:ApiKey"] ?? throw new InvalidOperationException("Finnhub ApiKey not found");
            _context = context;
        }

        public async Task<Result<Stock>> GetStockFromApiAsync(string ticker)
        {
            var normalizedTicker = ticker.Trim().ToUpperInvariant();
              
            var url = $"https://finnhub.io/api/v1/quote?symbol={ticker}&token={_apiKey}";

            var response = await _httpClient.GetFromJsonAsync<FinnhubQuote>(url);

            if (response?.CurrentPrice is null || response.CurrentPrice <= 0)
            {
                return Result<Stock>.Fail("negative price stock");
            }

            var stock =new Stock
            {
                Ticker = normalizedTicker,
                CurrentPrice = response.CurrentPrice,
                PriceChange = response.Change,
                LastUpdated = DateTime.UtcNow
            };
            return Result<Stock>.Ok(stock);
        }

    }

    public class FinnhubQuote
    {
        public decimal? c { get; set; }
        public decimal? d { get; set; }
        public decimal? CurrentPrice => c;
        public decimal? Change => d;
    }
}