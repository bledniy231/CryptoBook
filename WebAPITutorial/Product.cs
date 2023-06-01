namespace WebAPITutorial
{
	public class Product
	{
		public int Id { get; set; }
		public int Exchange { get; set; }
		public string Symbol { get; set; } = null!;
		public decimal? LastPrice { get; set; }
		public decimal? BaseVolume { get; set; }
		public decimal? QuoteVolume { get; set; }
		public double? Volatility { get; set; }
		public decimal? Liquidity { get; set; }
		public decimal? PriceChange { get; set; }
		public decimal? PriceChangePercent { get; set; }
	}
}
