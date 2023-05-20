using CryptoExchange.Net.CommonObjects;
using System.Collections;

namespace WebAPITutorial
{
	public abstract class BaseExchange
	{
		public abstract int id { get; }
		public abstract string name { get; }
		public async Task<IEnumerable<Product>> GetListRUBAsync()
		{
			var t =  await Task.Run(() => { return GetTickersRUB(); });
			return t;
		}
		public async Task<IEnumerable<Product>> GetListUSDTAsync()
		{
			var t = await Task.Run(() => { return GetTickersUSDT(); });
			return t;
		}
		public async Task<List<Kline>> GetKlinesCommonSpotClientAsync(string? symbol, int intervalMin, int periodOfHours)
		{
			var t = await Task.Run(() => { return GetKlinesCommonSpotClient(symbol, intervalMin, periodOfHours); });
			return t;
		}
		public async Task<IEnumerable> GetKlinesExchangeDataAsync(string? symbol, int periodOfHours)
		{
			var t = await Task.Run(() => { return GetKlinesExchangeData(symbol, periodOfHours); });
			return t;
		}

		protected abstract IEnumerable<Product> GetTickersRUB();
		protected abstract IEnumerable<Product> GetTickersUSDT();
		protected abstract Product ToProduct(object product, decimal usdCurrency);
		protected abstract List<Kline> GetKlinesCommonSpotClient(string? symbol, int intervalMin, int periodOfHours);
		protected abstract IEnumerable GetKlinesExchangeData(string? symbol, int periodOfHours);
	}
}
