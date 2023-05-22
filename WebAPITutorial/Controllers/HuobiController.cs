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
		private readonly ExchangeControllerHelper _helper;
		public HuobiController(HuobiExchange exchange, ExchangeControllerHelper helper)
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
		public async Task<ActionResult<List<HuobiKline>>> GetKlinesExcData(string symbol, int periodOfHours)
		{
			return await _helper.GetKlinesExcDataAsync(symbol, periodOfHours, _exchange.GetKlinesExchangeDataAsync<HuobiKline>);
		}

		[HttpGet("{symbol}/{intervalMin}/{periodOfHours}")]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<ActionResult<List<Kline>>> GetKlinesComSpotCl(string symbol, int intervalMin, int periodOfHours)
		{
			return await _helper.GetKlinesComSpotClAsync(symbol, intervalMin, periodOfHours, _exchange.GetKlinesCommonSpotClientAsync);
		}
	}
}
