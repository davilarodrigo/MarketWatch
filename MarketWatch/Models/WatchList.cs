using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarketWatch.Models
{
    public class Watchlist
    {
        public int Id { get; set; }
        public List<string> Tickers { get; set; } = new List<string>();

        [NotMapped]
        public List<Stock> Stocks { get; set; } = new List<Stock>();
    }
}