using Binance.Net.Objects.Models.Spot;
using CryptoExchange.Net.CommonObjects;
using Microsoft.AspNetCore.Mvc;
using WebAPITutorial.Exchanges;

namespace WebAPITutorial.Controllers
{
	[ApiController]
	[Route("api/[controller]/[action]")]
	public class BinanceController : ControllerBase
	{
		private readonly BinanceExchange _exchange;
		private readonly ExchangeControllerHelper _helper;
		public BinanceController(BinanceExchange exchange, ExchangeControllerHelper helper)
		{
			_exchange = exchange;
			_helper = helper;
		}

		[HttpGet]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<ActionResult<IEnumerable<Product>>> GetTickers()
		{
			return await _helper.GetTickersAsync(_exchange.GetTickersAsync);
		}

		[HttpGet("{symbol}")]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<ActionResult<IEnumerable<Product>>> GetExactTicker(string symbol)
		{
			return await _helper.GetExactTickerAsync(symbol, _exchange.GetExactTickerAsync);
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
		public async Task<ActionResult<List<BinanceSpotKline>>> GetKlinesExcData(string symbol, int periodOfHours)
		{
			return await _helper.GetKlinesExcDataAsync(symbol, periodOfHours, _exchange.GetKlinesExchangeDataAsync<BinanceSpotKline>);
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
