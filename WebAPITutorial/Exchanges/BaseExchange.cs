using CryptoExchange.Net.CommonObjects;
using System.Runtime.CompilerServices;

namespace WebAPITutorial
{
	public abstract class BaseExchange
	{
		public abstract int id { get; }
		public abstract string name { get; }

		public abstract Task<IEnumerable<Product>> GetTickersRUBAsync();
		public abstract Task<IEnumerable<Product>> GetTickersAsync();
		public abstract Task<Product> GetExactTickerAsync(string symbol, bool isRub);
		public abstract Task<List<Kline>> GetKlinesCommonSpotClientAsync(string symbol, int intervalMin, int periodOfHours);
		public abstract Task<List<T>> GetKlinesExchangeDataAsync<T>(string symbol, int periodOfHours);
		protected abstract Product ToProduct(object product, decimal usdCurrency);
	}
}
