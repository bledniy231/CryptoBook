using CryptoExchange.Net.CommonObjects;
using Huobi.Net.Clients;
using Huobi.Net.Objects.Models;

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

		public override async Task<IEnumerable<Product>> GetTickersAsync()
		{
			var result = await client.SpotApi.ExchangeData.GetTickersAsync();
			List<Product> products = new List<Product>();

			if (result.Success)
				result.Data.Ticks.Where(p => pairsUSDT.Contains(p.Symbol)).ToList().ForEach(p => products.Add(ToProduct(p)));
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

		public override async Task<IEnumerable<Product>> GetTickersRUBAsync()
		{
			var result = await client.SpotApi.ExchangeData.GetTickersAsync();
			List<Product> products = new List<Product>();

			if (result.Success)
				result.Data.Ticks.Where(p => pairsUSDT.Contains(p.Symbol)).ToList().ForEach(p => products.Add(ToProduct(p, usdCurrency)));
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

		public override async Task<List<HuobiKline>> GetKlinesExchangeDataAsync<HuobiKline>(string symbol, int periodOfHours)
		{
			if (!pairsUSDT.Contains(symbol)) return new List<HuobiKline>();
			var result = await client.SpotApi.ExchangeData.GetKlinesAsync(symbol, Huobi.Net.Enums.KlineInterval.OneHour);
			return (List<HuobiKline>)result.Data;
		}

		protected override Product ToProduct(object product, decimal usdCurrency = 1)
		{
			HuobiSymbolTick huobiProduct = (HuobiSymbolTick)product;
			Product p = new Product();
			AveragePrice averagePrice = new AveragePrice();
			p.Id = ++idProduct;
			p.Symbol = huobiProduct.Symbol;
			p.Exchange = id;
			p.LastPrice = huobiProduct.ClosePrice * usdCurrency;
			p.BaseVolume = huobiProduct.Volume;
			p.QuoteVolume = huobiProduct.QuoteVolume;
			p.Liquidity = GetLiquidity(huobiProduct.Symbol, averagePrice).Result;
			p.Volatility = GetVolatility(huobiProduct, averagePrice);
			p.PriceChange = huobiProduct.ClosePrice - huobiProduct.OpenPrice;
			if (huobiProduct.ClosePrice != null && huobiProduct.OpenPrice != null)
			{
				p.PriceChangePercent = Math.Abs((decimal)
					((huobiProduct.ClosePrice - huobiProduct.OpenPrice) / huobiProduct.OpenPrice * 100));
			}
			else p.PriceChangePercent = null;
			return p;
		}

		private double GetVolatility(HuobiSymbolTick huobiProduct, AveragePrice averPr)
		{
			double result = 0;
			if (huobiProduct.ClosePrice != null && huobiProduct.TradeCount != null)
				result = Math.Sqrt(
					(((double)huobiProduct.ClosePrice - averPr.averagePrice) *
					((double)huobiProduct.ClosePrice - averPr.averagePrice) /
					(double)huobiProduct.TradeCount));
			return Double.IsNormal(result) ? Math.Round(result, 5) : 0;
		}
		private async Task<decimal> GetLiquidity(string symbol, AveragePrice averPr)
		{
			var result = await client.SpotApi.ExchangeData.GetOrderBookAsync(symbol, 1, 10);
			decimal asks = 0;
			decimal bids = 0;

			if (result.Success)
			{
				if (result.Data.Asks.Count() != 0 && result.Data.Bids.Count() != 0)
				{
					averPr.averagePrice = (double)result.Data.Asks.Sum(b => b.Price) / result.Data.Asks.Count();
					bids = result.Data.Bids.Sum(b => b.Price) / result.Data.Bids.Count() * result.Data.Bids.Sum(b => b.Quantity);
					asks = result.Data.Asks.Sum(a => a.Price) / result.Data.Asks.Count() * result.Data.Asks.Sum(a => a.Quantity);
				}
			}

			return asks > 0 ? Math.Round(bids / asks, 5) : 0;
		}

		private class AveragePrice
		{
			public double averagePrice = 0;
		}
	}
}
