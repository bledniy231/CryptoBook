using CryptoExchange.Net.CommonObjects;
using Huobi.Net.Objects.Models;
using Microsoft.AspNetCore.Mvc;
using WebAPITutorial.Exchanges;

namespace WebAPITutorial.Controllers
{
	[ApiController]
	[Route("[controller]/[action]")]
	public class HuobiController : ControllerBase
	{
		private readonly HuobiExchange _exchange;
		public HuobiController(HuobiExchange exchange)
		{
			_exchange = exchange;
		}

		[HttpGet]
		public async Task<IEnumerable<Product>> GetTickersRUBs()
		{
			var result = await _exchange.GetListRUBAsync();
			return result;
		}

		[HttpGet]
		public async Task<IEnumerable<Product>> GetTickersUSDTs()
		{
			var result = await _exchange.GetListUSDTAsync();
			return result;
		}

		[HttpGet("{symbol}/{periodOfHours}")]
		public async Task<IEnumerable<HuobiKline>> GetKlinesExcData(string symbol, int periodOfHours)
		{
			var result = await _exchange.GetKlinesExchangeDataAsync(symbol, periodOfHours);
			return (IEnumerable<HuobiKline>)result;
		}

		[HttpGet("{symbol}/{intervalMin}/{periodOfHours}")]
		public async Task<List<Kline>> GetKlinesComSpotCl(string symbol, int intervalMin, int periodOfHours)
		{
			var result = await _exchange.GetKlinesCommonSpotClientAsync(symbol, intervalMin, periodOfHours);
			return result;
		}
	}
}
