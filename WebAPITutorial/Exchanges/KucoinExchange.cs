using Kucoin.Net.Clients;
using Kucoin.Net.Objects.Models.Spot;
using CryptoExchange.Net.CommonObjects;

namespace WebAPITutorial.Exchanges
{
	public class KucoinExchange : BaseExchange
	{
		public KucoinExchange() 
		{
			string path = "DollarCurrency\\ActualDollarCurrency.txt";
			string? currstr = File.ReadLines(path).ElementAtOrDefault(0);

			if (currstr != null && Decimal.TryParse(currstr, out usdCurrency))
			{
				Console.WriteLine($"Read currency in KucoinExchange success: {usdCurrency}");
			}
		}

		private decimal usdCurrency;

		private int idProduct = 0;
		public override int id => 2;
		public override string name => "Kucoin";

		public KucoinClient client = new KucoinClient();

		private readonly List<string> pairsUSDT = new List<string>()
		{
			"BTC-USDT",
			"ETH-USDT",
			"BNB-USDT",
			"XRP-USDT",
			"UNI-USDT",
			"LTC-USDT",
			"DOGE-USDT",
			"MATIC-USDT",
			"SOL-USDT",
			"ARB-USDT"
		};

		public Dictionary<string, double> volatilityToday = new Dictionary<string, double>();

		public override async Task<IEnumerable<Product>> GetTickersAsync()
		{
			var result = await client.SpotApi.ExchangeData.GetTickersAsync();
			List<Product> products = new List<Product>();

			if (result.Success)
				result.Data.Data.Where(p => pairsUSDT.Contains(p.Symbol)).ToList().ForEach(p => products.Add(ToProduct(p)));

			idProduct = 0;
			return products;
		}

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

		public  override async Task<IEnumerable<Product>> GetTickersRUBAsync()
		{
			var result = await client.SpotApi.ExchangeData.GetTickersAsync();
			List<Product> products = new List<Product>();

			if (result.Success)
				result.Data.Data.Where(p => pairsUSDT.Contains(p.Symbol)).ToList().ForEach(p => products.Add(ToProduct(p, usdCurrency)));

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

		public override async Task<List<KucoinKline>> GetKlinesExchangeDataAsync<KucoinKline>(string symbol, int periodOfHours)
		{
			if (!pairsUSDT.Contains(symbol)) return new List<KucoinKline>();
			var result = await client.SpotApi.ExchangeData.GetKlinesAsync(symbol, Kucoin.Net.Enums.KlineInterval.OneMinute, DateTime.Now.AddHours(-1 * periodOfHours), DateTime.Now);
			return (List<KucoinKline>)result.Data;
		}


		protected override Product ToProduct(object product, decimal usdCurrency = 1)
		{
			KucoinAllTick kucoinProduct = (KucoinAllTick)product;
			Product p = new Product();
			p.Id = ++idProduct;
			p.Symbol = kucoinProduct.Symbol;
			p.Exchange = id;
			p.LastPrice = kucoinProduct.LastPrice * usdCurrency;
			p.BaseVolume = kucoinProduct.Volume;
			p.QuoteVolume = kucoinProduct.QuoteVolume;
			p.Volatility = volatilityToday[kucoinProduct.Symbol];
			p.Liquidity = GetLiquidity(kucoinProduct.Symbol).Result;
			p.PriceChange = kucoinProduct.ChangePrice;
			p.PriceChangePercent = kucoinProduct.ChangePercentage;
			return p;
		}

		// Useless
		private double GetVolatility(KucoinAllTick kucoinProduct)
		{
			double result = 0;
			if (kucoinProduct.AveragePrice != null && kucoinProduct.LastPrice != null)
				result = Math.Sqrt(
					(double)(kucoinProduct.LastPrice - kucoinProduct.AveragePrice) *
					(double)(kucoinProduct.LastPrice - kucoinProduct.AveragePrice));
				/*kucoinProduct.*/
				//TODO: реализация метода взвешенной волотильности
			return Double.IsNormal(result) ? Math.Round(result, 5) : 0;
		}
		private async Task<decimal> GetLiquidity(string symbol)
		{
			var result = await client.SpotApi.ExchangeData.GetAggregatedPartialOrderBookAsync(symbol, 20);
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
