using MarketWatch.Models;
using MarketWatch.Services;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;

namespace MarketWatch.Services
{
    public class FinnhubStockService : IStockService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public FinnhubStockService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["Finnhub:ApiKey"] ?? throw new InvalidOperationException("Finnhub ApiKey not found"); ;
        }

        public async Task<Stock> GetStockByTickerAsync(string ticker)
        {
            //rewrite ticker in uppercase for the finnhub api (does not work otherwise)
            ticker = ticker.ToUpper();

            var url = $"https://finnhub.io/api/v1/quote?symbol={ticker}&token={_apiKey}";

            try
            {
                var response = await _httpClient.GetFromJsonAsync<FinnhubQuote>(url);

                if (response == null || response.CurrentPrice == 0)
                    return null;

                return new Stock
                {
                    Ticker = ticker,
                    CurrentPrice = response.CurrentPrice,
                    PriceChange = response.Change
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }

        }
    }

    public class FinnhubQuote
    {
        //Example of api response
        //"{\"c\":0,\"d\":null,\"dp\":null,\"h\":0,\"l\":0,\"o\":0,\"pc\":0,\"t\":0}"
        //"{\"c\":298.05,\"d\":2.1,\"dp\":0.7096,\"h\":300.57,\"l\":295.62,\"o\":298.12,\"pc\":295.95,\"t\":1781812800}"
        public decimal? c { get; set; } // Current price
        public decimal? d { get; set; } // Change
        public decimal? CurrentPrice => c;
        public decimal? Change => d;
    }
}