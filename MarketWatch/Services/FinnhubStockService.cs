using MarketWatch.Models;
using MarketWatch.Services;
using MarketWatch.Data; 
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;

namespace MarketWatch.Services
{
    public class FinnhubStockService : IStockService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly ApplicationDbContext _context; // Add EF Core context

        // add DbContext 
        public FinnhubStockService(HttpClient httpClient, IConfiguration configuration, ApplicationDbContext context)
        {
            _httpClient = httpClient;
            _apiKey = configuration["Finnhub:ApiKey"] ?? throw new InvalidOperationException("Finnhub ApiKey not found");
            _context = context;
        }

        public async Task<Stock> GetStockByTickerAsync(string ticker)
        {
            ticker = ticker.ToUpper();

            // first search bd
            var existingStock = await _context.Stocks
                .FirstOrDefaultAsync(s => s.Ticker == ticker);

            // if it is in the BD and it is recent data (<1h) return it
            if (existingStock != null && (DateTime.UtcNow - existingStock.LastUpdated) < TimeSpan.FromHours(1))
            {
                return existingStock;
            }

            // if there is no data or it is old, do request to api
            var url = $"https://finnhub.io/api/v1/quote?symbol={ticker}&token={_apiKey}";

            try
            {
                var response = await _httpClient.GetFromJsonAsync<FinnhubQuote>(url);

                if (response == null || response.CurrentPrice == 0)
                    return null;

                // 4. Actualizar o Crear el registro en la Base de Datos
                if (existingStock != null)
                {
                    // Actualizamos el existente
                    existingStock.CurrentPrice = (decimal)response.CurrentPrice;
                    existingStock.PriceChange = response.Change;
                    existingStock.LastUpdated = DateTime.UtcNow;

                    _context.Stocks.Update(existingStock);
                }
                else
                {
                    // Creamos uno nuevo
                    existingStock = new Stock
                    {
                        Ticker = ticker,
                        CurrentPrice = (decimal)response.CurrentPrice,
                        PriceChange = response.Change,
                        LastUpdated = DateTime.UtcNow
                    };

                    _context.Stocks.Add(existingStock);
                }

                // 5. Guardar los cambios en la BD
                await _context.SaveChangesAsync();

                return existingStock;
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
        public decimal? c { get; set; }
        public decimal? d { get; set; }
        public decimal? CurrentPrice => c;
        public decimal? Change => d;
    }
}