using Binance.Net.Clients;
using Binance.Net.Objects.Models.Spot;
using CryptoExchange.Net.CommonObjects;

namespace WebAPITutorial.Exchanges
{
	public class BinanceExchange : BaseExchange
	{
		public BinanceExchange() 
		{
			string path = "DollarCurrency\\ActualDollarCurrency.txt";
			string? currstr = File.ReadLines(path).ElementAtOrDefault(0);

			if (currstr != null && Decimal.TryParse(currstr, out usdCurrency))
			{
				Console.WriteLine($"Read currency in BinanceExchange success: {usdCurrency}");
			}
		}

		private decimal usdCurrency;

		private int idProduct = 0;
		public override int id => 1;
		public override string name => "Binance";

		private readonly List<string> pairsUSDT = new List<string>() 
		{
			"BTCUSDT",
			"ETHUSDT",
			"BNBUSDT",
			"XRPUSDT",
			"UNIUSDT",
			"LTCUSDT",
			"DOGEUSDT",
			"MATICUSDT",
			"SOLUSDT",
			"ARBUSDT"
		};

		BinanceClient client = new BinanceClient();

		public override async Task<Product> GetExactTickerAsync(string symbol, bool isRub)
		{
			var result = await client.SpotApi.ExchangeData.GetTickerAsync(symbol);

			if (result.Success)
				if (!isRub)
					return ToProduct(result.Data);
				else
					return ToProduct(result.Data, usdCurrency);

			return new Product();
		}

		public override async Task<IEnumerable<Product>> GetTickersAsync()
		{
			var result = await client.SpotApi.ExchangeData.GetTickersAsync(pairsUSDT);
			List<Product> products = new List<Product>();

			if (result.Success)
				result.Data.ToList().ForEach(p => products.Add(ToProduct(p)));
			idProduct = 0;
			return products;
		}

		public override async Task<IEnumerable<Product>> GetTickersRUBAsync()
		{
			var result = await client.SpotApi.ExchangeData.GetTickersAsync(pairsUSDT);
			List<Product> products = new List<Product>();

			if (result.Success)
				result.Data.ToList().ForEach(p => products.Add(ToProduct(p, usdCurrency)));
			idProduct = 0;
			return products;
		}

		public override async Task<List<Kline>> GetKlinesCommonSpotClientAsync(string symbol, int intervalMin, int periodOfHours)
		{
			if (!pairsUSDT.Contains(symbol)) return new List<Kline>();
			List<Kline> klines = new List<Kline>();
			int minutes = intervalMin % 60;
			int hours = (intervalMin - minutes) / 60;
			TimeSpan timeSpan = new TimeSpan(hours, minutes, 0);
			var result = await client.SpotApi.CommonSpotClient.GetKlinesAsync(symbol, timeSpan, DateTime.Now.AddHours(-1 * periodOfHours), DateTime.Now);
			if (result.Success && result.Data.Count() > 0)
				klines = result.Data.ToList();
			return klines;
		}

		public override async Task<List<BinanceSpotKline>> GetKlinesExchangeDataAsync<BinanceSpotKline>(string symbol, int periodOfHours)
		{
			if (!pairsUSDT.Contains(symbol)) return new List<BinanceSpotKline>();
			var result = await client.SpotApi.ExchangeData.GetKlinesAsync(symbol, Binance.Net.Enums.KlineInterval.OneHour, DateTime.Now.AddHours(-1 * periodOfHours), DateTime.Now);
			return (List<BinanceSpotKline>)result.Data;
		}

		protected override Product ToProduct(object product, decimal usdCurrency = 1)
		{
			Binance24HPrice binanceProduct = (Binance24HPrice)product;
			Product p = new Product();
			p.Id = ++idProduct;
			p.Symbol = binanceProduct.Symbol;
			p.Exchange = id;
			p.LastPrice = binanceProduct.LastPrice * usdCurrency;
			p.BaseVolume = binanceProduct.Volume;
			p.QuoteVolume = binanceProduct.QuoteVolume;
			p.Volatility = GetVolatility(binanceProduct);
			p.Liquidity = GetLiquidity(binanceProduct.Symbol).Result;
			p.PriceChange = binanceProduct.PriceChange;
			p.PriceChangePercent = binanceProduct.PriceChangePercent;
			return p;
		}


		private double GetVolatility(Binance24HPrice binanceProduct)
		{
			double result = Math.Sqrt(
				(double)(binanceProduct.LastPrice - binanceProduct.WeightedAveragePrice) *
				(double)(binanceProduct.LastPrice - binanceProduct.WeightedAveragePrice) /
				binanceProduct.TotalTrades);
			return Double.IsNormal(result) ? Math.Round(result, 5) : 0;
		}
		private async Task<decimal> GetLiquidity(string symbol)
		{
			var result = await client.SpotApi.ExchangeData.GetOrderBookAsync(symbol, 10);
			decimal asks = 0;
			decimal bids = 0;

			if (result.Success)
			{
				if (result.Data.Asks.Count() != 0 && result.Data.Bids.Count() != 0)
				{ 
					bids = result.Data.Bids.Sum(b => b.Price) / result.Data.Bids.Count() * result.Data.Bids.Sum(b => b.Quantity);
					asks = result.Data.Asks.Sum(a => a.Price) / result.Data.Asks.Count() * result.Data.Asks.Sum(a => a.Quantity);
				}
			}

			return asks > 0 ? Math.Round(bids / asks, 5) : 0;
		}
	}
}
