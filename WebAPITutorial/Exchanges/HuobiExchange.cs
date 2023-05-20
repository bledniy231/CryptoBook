using Binance.Net.Interfaces;
using Binance.Net.Objects.Models.Spot;
using CryptoExchange.Net.CommonObjects;
using Huobi.Net.Clients;
using Huobi.Net.Objects.Models;
using System.Collections;

namespace WebAPITutorial.Exchanges
{
	public class HuobiExchange : BaseExchange
	{
		private readonly List<string> pairsUSDT = new List<string>()
		{
			"btcusdt",
			"ethusdt",
			"bnbusdt",
			"xrpusdt",
			"uniusdt",
			"ltcusdt",
			"dogeusdt",
			"maticusdt",
			"solusdt",
			"arbusdt"
		};
		public HuobiExchange() 
		{
			string path = "DollarCurrency\\ActualDollarCurrency.txt";
			string? currstr = File.ReadLines(path).ElementAtOrDefault(0);

			if (currstr != null && Decimal.TryParse(currstr, out usdCurrency))
			{
				Console.WriteLine($"Read currency in HuobiExchange success: {usdCurrency}");
			}
		}

		private decimal usdCurrency;

		private int idProduct = 0;
		public override int id => 3;
		public override string name => "Huobi";

		HuobiClient client = new HuobiClient();

		protected override IEnumerable<Product> GetTickersUSDT()
		{
			var result = client.SpotApi.ExchangeData.GetTickersAsync().Result;
			List<Product> products = new List<Product>();
			
			if (result.Success)
				result.Data.Ticks.Where(p => pairsUSDT.Contains(p.Symbol)).ToList().ForEach(p => products.Add(ToProduct(p)));
			//TODO: Сохранение в базу данных
			idProduct = 0;
			return products;
		}

		protected override IEnumerable<Product> GetTickersRUB()
		{
			var result = client.SpotApi.ExchangeData.GetTickersAsync().Result;
			List<Product> products = new List<Product>();

			if (result.Success)
				result.Data.Ticks.Where(p => pairsUSDT.Contains(p.Symbol)).ToList().ForEach(p => products.Add(ToProduct(p, usdCurrency)));
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

		protected override IEnumerable GetKlinesExchangeData(string? symbol, int periodOfHours)
		{
			if (symbol == null) return new List<IBinanceKline> { new BinanceSpotKline() };
			var result = client.SpotApi.ExchangeData.GetKlinesAsync(symbol, Huobi.Net.Enums.KlineInterval.OneHour).Result;
			return result.Data;
		}

		protected override Product ToProduct(object product, decimal usdCurrency = 1)
		{
			HuobiSymbolTick huobiProduct = (HuobiSymbolTick)product;
			Product p = new Product();
			double averagePrice = 0;
			p.Id = ++idProduct;
			p.Symbol = huobiProduct.Symbol;
			p.Exchange = id;
			p.LastPrice = huobiProduct.ClosePrice * usdCurrency;
			p.BaseVolume = huobiProduct.Volume;
			p.QuoteVolume = huobiProduct.QuoteVolume;
			p.Liquidity = GetLiquidity(huobiProduct.Symbol, ref averagePrice);
			Console.WriteLine("ToPr" + averagePrice);
			p.Volatility = GetVolatility(huobiProduct, in averagePrice);
			p.PriceChange = huobiProduct.ClosePrice - huobiProduct.OpenPrice;
			if (huobiProduct.ClosePrice != null && huobiProduct.OpenPrice != null)
			{
				p.PriceChangePercent = Math.Abs((decimal)
					((huobiProduct.ClosePrice - huobiProduct.OpenPrice) / huobiProduct.OpenPrice * 100));
			}
			else p.PriceChangePercent = null;
			return p;
		}

		private double GetVolatility(HuobiSymbolTick huobiProduct, in double averPr)
		{
			double result = 0;
			Console.WriteLine("Vol" + averPr);
			if (huobiProduct.ClosePrice != null && huobiProduct.TradeCount != null)
				result = Math.Sqrt(
					(((double)huobiProduct.ClosePrice - averPr) *
					((double)huobiProduct.ClosePrice - averPr) /
					(double)huobiProduct.TradeCount));
			return Double.IsNormal(result) ? Math.Round(result, 5) : 0;
		}
		private decimal GetLiquidity(string symbol, ref double averPr)
		{
			var result = client.SpotApi.ExchangeData.GetOrderBookAsync(symbol, 1, 10).Result;
			decimal asks = 0;
			decimal bids = 0;

			if (result.Success)
			{
				if (result.Data.Asks.Count() != 0 && result.Data.Bids.Count() != 0)
				{
					averPr = (double)result.Data.Asks.Sum(b => b.Price) / result.Data.Asks.Count();
					Console.WriteLine("Liq" + averPr);
					bids = result.Data.Bids.Sum(b => b.Price) / result.Data.Bids.Count() * result.Data.Bids.Sum(b => b.Quantity);
					asks = result.Data.Asks.Sum(a => a.Price) / result.Data.Asks.Count() * result.Data.Asks.Sum(a => a.Quantity);
				}
			}

			return asks > 0 ? Math.Round(bids / asks, 5) : 0;
		}
	}
}
