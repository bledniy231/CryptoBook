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
		private readonly ExchangeControllerHelper _helper;
		public KucoinController(KucoinExchange exchange, ExchangeControllerHelper helper)
		{
			_exchange = exchange;
			_helper = helper;
		}

		[HttpGet]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<ActionResult<IEnumerable<Product>>> GetTickersUSDTs()
		{
			return await _helper.GetTickersUSDTAsync(_exchange.GetTickersUSDTAsync);
		}

		[HttpGet]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<ActionResult<IEnumerable<Product>>> GetTickersRUBs()
		{
			return await _helper.GetTickersRUBAsync(_exchange.GetTickersRUBAsync);
		}

		[HttpGet("{symbol}/{periodOfHours}")]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<ActionResult<List<KucoinKline>>> GetKlinesExcData(string symbol, int periodOfHours)
		{
			return await _helper.GetKlinesExcDataAsync(symbol, periodOfHours, _exchange.GetKlinesExchangeDataAsync<KucoinKline>);
		}

		[HttpGet("{symbol}/{intervalMin}/{periodOfHours}")]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<ActionResult<List<Kline>>> GetKlinesComSpotCl(string symbol, int intervalMin, int periodOfHours)
		{
			return await _helper.GetKlinesComSpotClAsync(symbol, intervalMin, periodOfHours, _exchange.GetKlinesCommonSpotClientAsync);
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
