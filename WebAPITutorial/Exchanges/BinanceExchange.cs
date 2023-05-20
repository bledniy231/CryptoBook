using Binance.Net.Clients;
using Binance.Net.Interfaces;
using Binance.Net.Objects.Models.Spot;
using CryptoExchange.Net.CommonObjects;

namespace WebAPITutorial.Exchanges
{
	public class BinanceExchange : BaseExchange
	{
		public BinanceExchange() { }

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

		private readonly List<string> pairsRUB = new List<string>()
		{
			"BTCRUB",
			"ETHRUB",
			"XRPRUB",
			"BNBRUB",
			"BUSDRUB",
			"USDTRUB",
			"LTCRUB",
			"ADARUB",
			"SHIBRUB",
			"MATICRUB",
			"DOTRUB",
			"SOLRUB",
			"ICPRUB",
			"TRURUB",
			"WAVESRUB",
			"ARPARUB",
			"FTMRUB",
			"NURUB",
			"ALGORUB",
			"NEORUB",
			"NEARRUB",
			"ARBRUB"
		};

		BinanceClient client = new BinanceClient();

		protected override IEnumerable<Product> GetTickersRUB()
		{
			var result = client.SpotApi.ExchangeData.GetTickersAsync(pairsRUB).Result;
			List<Product> products = new List<Product>();

			if (result.Success)
				result.Data.ToList().ForEach(p => products.Add(ToProduct(p)));
			//TODO: Сохранение в базу данных
			idProduct = 0;
			return products;
		}

		protected override IEnumerable<Product> GetTickersUSDT() //340 пар c долларом
		{
			var result = client.SpotApi.ExchangeData.GetTickersAsync(pairsUSDT).Result;
			List<Product> products = new List<Product>();

			if (result.Success)
				result.Data.ToList().ForEach(p => products.Add(ToProduct(p)));
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

		protected override IEnumerable<IBinanceKline> GetKlinesExchangeData(string? symbol, int periodOfHours)
		{
			if (symbol == null) return new List<IBinanceKline> { new BinanceSpotKline() };
			var result = client.SpotApi.ExchangeData.GetKlinesAsync(symbol, Binance.Net.Enums.KlineInterval.OneHour, DateTime.Now.AddHours(-1 * periodOfHours), DateTime.Now).Result;
			return result.Data;
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
			p.Volatility = GetVolatility(binanceProduct);		//binanceProduct.LowPrice > 0 ? (binanceProduct.HighPrice - binanceProduct.LowPrice) / binanceProduct.LowPrice : 0;
			p.Liquidity = GetLiquidity(binanceProduct.Symbol);	//binanceProduct.Volume > 0 ? binanceProduct.QuoteVolume / binanceProduct.Volume : 0;
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
		private decimal GetLiquidity(string symbol)
		{
			var result = client.SpotApi.ExchangeData.GetOrderBookAsync(symbol, 10).Result;
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
