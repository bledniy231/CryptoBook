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

		protected override IEnumerable<Product> GetTickersUSDT()
		{
			var result = client.SpotApi.ExchangeData.GetTickersAsync().Result;
			List<Product> products = new List<Product>();

			if (result.Success)
				result.Data.Data.Where(p => pairsUSDT.Contains(p.Symbol)).ToList().ForEach(p => products.Add(ToProduct(p)));
			//TODO: Сохранение в базу данных
			idProduct = 0;
			return products;
		}

		protected override IEnumerable<Product> GetTickersRUB()
		{
			var result = client.SpotApi.ExchangeData.GetTickersAsync().Result;
			List<Product> products = new List<Product>();

			if (result.Success)
				result.Data.Data.Where(p => pairsUSDT.Contains(p.Symbol)).ToList().ForEach(p => products.Add(ToProduct(p, usdCurrency)));
			//TODO: Сохранение в базу данных
			idProduct = 0;
			return products;
		}

		protected override List<Kline> GetKlinesCommonSpotClient(string? symbol, int intervalMin, int periodOfHours)
		{
			if (symbol == null) return new List<Kline>();
			List<Kline> klines = new List<Kline>();
			int minutes = intervalMin % 60;
			int hours = (intervalMin - minutes) / 60;
			TimeSpan timeSpan = new TimeSpan(hours, minutes, 0);
			var result = client.SpotApi.CommonSpotClient.GetKlinesAsync(symbol, timeSpan, DateTime.Now.AddHours(-1 * periodOfHours), DateTime.Now).Result;
			if (result.Success && result.Data.Count() > 0)
				klines = result.Data.ToList();
			return klines;
		}

		protected override IEnumerable<KucoinKline> GetKlinesExchangeData(string? symbol, int periodOfHours)
		{
			if (symbol == null) throw new ArgumentNullException("Symbol is not defined");
			var result = client.SpotApi.ExchangeData.GetKlinesAsync(symbol, Kucoin.Net.Enums.KlineInterval.OneMinute, DateTime.Now.AddHours(-1 * periodOfHours), DateTime.Now).Result;
			return result.Data;
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
			//if (kucoinProduct.Symbol == "BTC-USDT")
				p.Volatility = volatilityToday[kucoinProduct.Symbol];  //GetVolatility(kucoinProduct);
			//else
				//p.Volatility = 0;
			p.Liquidity = GetLiquidity(kucoinProduct.Symbol);
			p.PriceChange = kucoinProduct.ChangePrice;
			p.PriceChangePercent = kucoinProduct.ChangePercentage;
			return p;
		}

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
		private decimal GetLiquidity(string symbol)
		{
			var result = client.SpotApi.ExchangeData.GetAggregatedPartialOrderBookAsync(symbol, 20).Result;
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
