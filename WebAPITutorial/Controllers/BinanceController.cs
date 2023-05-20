using Binance.Net.Interfaces;
using CryptoExchange.Net.CommonObjects;
using Microsoft.AspNetCore.Mvc;
using WebAPITutorial.Exchanges;

namespace WebAPITutorial.Controllers
{
	[ApiController]
	[Route("[controller]/[action]")]
	public class BinanceController : ControllerBase
	{
		private readonly BinanceExchange _exchange;
		public BinanceController(BinanceExchange exchange)
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
		public async Task<IEnumerable<IBinanceKline>> GetKlinesExcData(string symbol, int periodOfHours)
		{
			var result = await _exchange.GetKlinesExchangeDataAsync(symbol, periodOfHours);
			return (IEnumerable<IBinanceKline>)result;
		}

		[HttpGet("{symbol}/{intervalMin}/{periodOfHours}")]
		public async Task<List<Kline>> GetKlinesComSpotCl(string symbol, int intervalMin, int periodOfHours)
		{
			var result = await _exchange.GetKlinesCommonSpotClientAsync(symbol, intervalMin, periodOfHours);
			return result;
		}
	}

	/*[ApiController]
	[Route("commonSpotClientBinance/[controller]")]
	public class GetKlinesCSCBinanceController : ControllerBase
	{
		BinanceExchange exchange = new BinanceExchange();
		[HttpGet("{symbol}/{intervalMin}/{periodOfHours}")]
		public async Task<List<Kline>> Get(string symbol, int intervalMin, int periodOfHours)
		{
			var result = await exchange.GetKlinesCommonSpotClientAsync(symbol, intervalMin, periodOfHours);
			return result;
		}
	}*/
}
