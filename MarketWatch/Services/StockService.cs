using MarketWatch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarketWatch.Data;
using Microsoft.EntityFrameworkCore;


namespace MarketWatch.Services
{
    public class StockService : IWatchlistService
    {
        private readonly List<Watchlist> _watchlists = new();
        private int _nextId = 1;

        private readonly ApplicationDbContext _context;
        private readonly IStockService _stockService;

        public StockService(ApplicationDbContext context, IStockService stockService)
        {
            _context = context;
            _stockService = stockService;
        }

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
                    watchlist.Stocks.Add(stockInfo);
                }
            }

            return watchlist;
        }

        public async Task<Watchlist> CreateWatchlistAsync(List<string> tickers)
        {
            var newWatchlist = new Watchlist
            {
                Tickers = tickers ?? new List<string>()
            };


            _context.Watchlists.Add(newWatchlist);
            await _context.SaveChangesAsync(); // generate id in db


            newWatchlist.Stocks = new List<Stock>();
            foreach (var ticker in newWatchlist.Tickers)
            {
                var stockInfo = await _stockService.GetStockByTickerAsync(ticker);
                if (stockInfo != null)
                {
                    newWatchlist.Stocks.Add(stockInfo);
                }
            }

            return newWatchlist;
        }

        public async Task<bool> DeleteWatchlistAsync(int id)
        {
            var watchlist = await _context.Watchlists.FirstOrDefaultAsync(w => w.Id == id);
            if (watchlist == null) return false;

            _context.Watchlists.Remove(watchlist);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}