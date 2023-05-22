using CryptoExchange.Net.CommonObjects;
using Microsoft.AspNetCore.Mvc;

namespace WebAPITutorial.Controllers
{
	public class ExchangeControllerHelper : ControllerBase
	{
		public async Task<ActionResult<IEnumerable<Product>>> GetTickersUSDTAsync(Func<Task<IEnumerable<Product>>> getTickersUSDTAsync)
		{
			Task<IEnumerable<Product>> task = getTickersUSDTAsync();
			var result = await task;
			if (task.IsCompletedSuccessfully)
				return Ok(result);
			else
				return BadRequest();
		}

		public async Task<ActionResult<IEnumerable<Product>>> GetTickersRUBAsync(Func<Task<IEnumerable<Product>>> getTickersRUBAsync)
		{
			Task<IEnumerable<Product>> task = getTickersRUBAsync();
			var result = await task;
			if (task.IsCompletedSuccessfully)
				return Ok(result);
			else
				return BadRequest();
		}

		public async Task<ActionResult<List<T>>> GetKlinesExcDataAsync<T>(string symbol, int periodOfHours, Func<string, int, Task<List<T>>> getKlinesExDataAsync)
		{
			if (periodOfHours < 0)
				return BadRequest("Period of hours less than 0");

			var task = getKlinesExDataAsync(symbol, periodOfHours);
			var result = await task;
			if (result.Cast<T>().Count() == 0)
				return BadRequest("No data for this period or symbol is not supported");
			else
				return Ok(result);
		}

		public async Task<ActionResult<List<Kline>>> GetKlinesComSpotClAsync(string symbol, int intervalMin, int periodOfHours,
			Func<string, int, int, Task<List<Kline>>> getKlinesComSpotClAsync)
		{
			if (periodOfHours < 0 || intervalMin < 0)
				return BadRequest("Period of hours or interval in minutes less than 0");

			var task = getKlinesComSpotClAsync(symbol, intervalMin, periodOfHours);
			var result = await task;
			if (result.Count() == 0)
				return BadRequest("No data for this period or symbol is not supported");
			else
				return Ok(result);
		}
	}
}
