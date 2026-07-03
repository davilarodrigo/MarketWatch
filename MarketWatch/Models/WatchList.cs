namespace MarketWatch.Models
{
    public class Watchlist
    {
        public int Id { get; set; }
        public List<string> Tickers { get; set; } = new List<string>();
    }
}