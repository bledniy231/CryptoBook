using CryptoExchange.Net.CommonObjects;
using Kucoin.Net.Objects.Models.Spot;
using Microsoft.AspNetCore.Mvc;
using WebAPITutorial.Exchanges;

namespace WebAPITutorial.Controllers
{
	[ApiController]
	[Route("[controller]/[action]")]
	public class KucoinController : ControllerBase
	{
		private readonly KucoinExchange _exchange;
		public KucoinController(KucoinExchange exchange)
		{
			_exchange = exchange;
		}
		[HttpGet]
		public async Task<IEnumerable<Product>> GetTickersUSDTs()
		{
			var result = await _exchange.GetListUSDTAsync();
			return result;
		}

		[HttpGet]
		public async Task<IEnumerable<Product>> GetTickersRUBs()
		{
			var result = await _exchange.GetListRUBAsync();
			return result;
		}

		[HttpGet("{symbol}/{periodOfHours}")]
		public async Task<IEnumerable<KucoinKline>> GetKlinesExcData(string symbol, int periodOfHours)
		{
			var result = await _exchange.GetKlinesExchangeDataAsync(symbol, periodOfHours);
			return (IEnumerable<KucoinKline>)result;
		}

		[HttpGet("{symbol}/{intervalMin}/{periodOfHours}")]
		public async Task<List<Kline>> GetKlinesComSpotCl(string symbol, int intervalMin, int periodOfHours)
		{
			var result = await _exchange.GetKlinesCommonSpotClientAsync(symbol, intervalMin, periodOfHours);
			return result;
		}
	}

	/*[ApiController]
	[Route("commonSpotClientKucoin/[controller]")]
	public class GetKlinesCSCKucoinController : ControllerBase
	{
		KucoinExchange exchange = new KucoinExchange();
		[HttpGet("{symbol}/{intervalMin}/{periodOfHours}")]
		public async Task<List<Kline>> Get(string symbol, int intervalMin, int periodOfHours)
		{
			var result = await exchange.GetKlinesCommonSpotClientAsync(symbol, intervalMin, periodOfHours);
			return result;
		}
	}*/
}
