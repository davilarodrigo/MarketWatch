using System.ComponentModel.DataAnnotations;

namespace MarketWatch.Models
{
    public class Stock
    {
        [Key]
        public string Ticker { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public decimal? CurrentPrice { get; set; }
        public decimal? PriceChange { get; set; }
        public decimal? PercentChange { get; set; }
        public long? Volume { get; set; }
        public decimal? MarketCap { get; set; }
        public DateTime? LastUpdated { get; set; }
    }
}