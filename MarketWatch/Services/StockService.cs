using MarketWatch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarketWatch.Data;
using Microsoft.EntityFrameworkCore;
using MarketWatch.Interfaces;
using ProductCatalogAPI.Common;


namespace MarketWatch.Services
{
    public class StockService : IStockService
    {
        private readonly List<Watchlist> _watchlists = new();
        private int _nextId = 1;

        private readonly ApplicationDbContext _context;
   
        private readonly IFinnhubService _finnhubService;
        private readonly TimeSpan _cacheDuration = new TimeSpan(12,0,0);


        public StockService(ApplicationDbContext context, IFinnhubService finnhubService)
        {
            _context = context;
 
            _finnhubService = finnhubService;
        }
        /*
        public async Task<Watchlist?> GetWatchlistAsync(int id)
        {
            var watchlist = await _context.Watchlists.FirstOrDefaultAsync(w => w.Id == id);
            if (watchlist == null) return null;

            //populate stocks property (which was marked as "not mapped")
            watchlist.Stocks = new List<Stock>();
            foreach (var ticker in watchlist.Tickers)
            {
                var stockInfo = await _stockService.GetStockByTickerAsync(ticker);
                if (stockInfo != null)
                {
                    //watchlist.Stocks.Add(stockInfo);//check later when i implement watchlists
                }
            }

            return watchlist;
        }*/


        public async Task<bool> DeleteWatchlistAsync(int id)
        {
            var watchlist = await _context.Watchlists.FirstOrDefaultAsync(w => w.Id == id);
            if (watchlist == null) return false;

            _context.Watchlists.Remove(watchlist);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Result<Stock>> GetStockByTickerAsync(string ticker)
        {
            var normalizedTicker = ticker.Trim().ToUpperInvariant();
           
            var existingStock = await _context.Stocks
                .FirstOrDefaultAsync(stock => stock.Ticker == normalizedTicker);

            if (existingStock?.LastUpdated is DateTime lastUpdated &&
             lastUpdated >= DateTime.UtcNow.Subtract(_cacheDuration))
            {
                return Result<Stock>.Ok(existingStock);
            }

            var latestStockResult = await _finnhubService.GetStockFromApiAsync(normalizedTicker);
            if (!latestStockResult.Success)
            {
                return Result<Stock>.Fail("Stock does not exist in finnhub database");
            }

            var latestStock = latestStockResult.Data;

            if (existingStock == null)
            {
                _context.Stocks.Add(latestStock);
            }
            else
            {
                existingStock.CompanyName = latestStock.CompanyName;
                existingStock.CurrentPrice = latestStock.CurrentPrice;
                existingStock.PriceChange = latestStock.PriceChange;
                existingStock.PercentChange = latestStock.PercentChange;
                existingStock.Volume = latestStock.Volume;
                existingStock.MarketCap = latestStock.MarketCap;
                existingStock.LastUpdated = latestStock.LastUpdated;

                latestStock = existingStock;
            }

            await _context.SaveChangesAsync();

            return Result<Stock>.Ok(latestStock);
        }
    }
}