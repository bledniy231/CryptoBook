using CryptoExchange.Net.CommonObjects;

namespace WebAPITutorial
{
	public abstract class BaseExchange
	{
		public abstract int id { get; }
		public abstract string name { get; }
		//public IEnumerable<Product> GetListRUB()
		//{
		//	return GetTickersRUBAsync().Result;
		//}
		//public async Task<IEnumerable<Product>> GetListUSDTAsync()
		//{
		//	return await GetTickersUSDTAsync();
		//}
		//public async Task<List<Kline>> GetKlinesCSCAsync(string symbol, int intervalMin, int periodOfHours)
		//{
		//	var t = await Task.Run(() => { return GetKlinesCommonSpotClientAsync(symbol, intervalMin, periodOfHours); });
		//	return t;
		//}
		//public async Task<IEnumerable> GetKlinesExDataAsync<T>(string symbol, int periodOfHours)
		//{
		//	var t = await Task.Run(() => { return GetKlinesExchangeDataAsync<T>(symbol, periodOfHours); });
		//	return t;
		//}

		public abstract Task<IEnumerable<Product>> GetTickersRUBAsync();
		public abstract Task<IEnumerable<Product>> GetTickersUSDTAsync();
		public abstract Task<List<Kline>> GetKlinesCommonSpotClientAsync(string symbol, int intervalMin, int periodOfHours);
		public abstract Task<List<T>> GetKlinesExchangeDataAsync<T>(string symbol, int periodOfHours);
		protected abstract Product ToProduct(object product, decimal usdCurrency);
	}
}
