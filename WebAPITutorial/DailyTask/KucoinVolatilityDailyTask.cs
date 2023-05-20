using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebAPITutorial.DBContext;
using WebAPITutorial.Exchanges;
using WebAPITutorial.Repos;
using static WebAPITutorial.Models.KucoinVolatilityEntity;

namespace WebAPITutorial.DailyTask
{
	public class KucoinVolatilityDailyTask : IDailyTask
	{
		private KucoinExchange _exchange;
		private IServiceProvider _serviceProvider;
		//private KucoinVolContext _dataContext;
		//private KucoinVolRepos _repos = new KucoinVolRepos();

		private readonly List<string> pairs = new List<string>()
		{
			"BTC-USDT",
			"ETH-USDT",
			"BNB-USDT",
			"XRP-USDT",
			"UNI-USDT",
			"LTC-USDT",
			"DOGE-USDT",
			"MATIC-USDT",
			"SOL-USDT",
			"ARB-USDT"
		};
		public KucoinVolatilityDailyTask(KucoinExchange exchange, IServiceProvider serviceProvider)
		{
			_exchange = exchange;
			_serviceProvider = serviceProvider;
		}

		public async Task DoTaskAsync()
		{
			var result = await _exchange.client.SpotApi.ExchangeData.GetTickersAsync();

			if (result.Success)
			{
				result.Data.Data.Where(p => pairs.Contains(p.Symbol)).ToList().ForEach(p =>
				{
					using (var scope = _serviceProvider.CreateScope())
					{
						double sumQV, denominator, numerator;
						var _dataContext = scope.ServiceProvider.GetRequiredService<KucoinVolContext>();
						switch (p.Symbol)
						{
							case "BTC-USDT":
								if (p.QuoteVolume != null && p.ChangePercentage != null)
									_dataContext.KucoinVolBTC.Add(new BTC_USDT_Item
									{
										QuoteVolume = (double)p.QuoteVolume,
										ChangePercentage = (double)p.ChangePercentage
									});
								else
									_dataContext.KucoinVolBTC.Add(new BTC_USDT_Item
									{
										QuoteVolume = 0,
										ChangePercentage = 0
									});
								_dataContext.SaveChanges();

								var listBTC = _dataContext.KucoinVolBTC.ToList();

								sumQV = listBTC.Sum(btc => btc.QuoteVolume);
								denominator = listBTC.Sum(btc => btc.QuoteVolume * btc.ChangePercentage) / sumQV;
								numerator = Math.Sqrt(listBTC.Sum(btc => btc.QuoteVolume * (btc.ChangePercentage - denominator * denominator)) / sumQV);

								if (!_exchange.volatilityToday.ContainsKey(p.Symbol))
									_exchange.volatilityToday.Add(p.Symbol, numerator / denominator);
								else _exchange.volatilityToday[p.Symbol] = numerator / denominator;

								listBTC.Clear();
								break;

							case "ETH-USDT":
								if (p.QuoteVolume != null && p.ChangePercentage != null)
									_dataContext.KucoinVolETH.Add(new ETH_USDT_Item
									{
										QuoteVolume = (double)p.QuoteVolume,
										ChangePercentage = (double)p.ChangePercentage
									});
								else
									_dataContext.KucoinVolETH.Add(new ETH_USDT_Item
									{
										QuoteVolume = 0,
										ChangePercentage = 0
									});
								_dataContext.SaveChanges();

								var listETH = _dataContext.KucoinVolETH.ToList();

								sumQV = listETH.Sum(eth => eth.QuoteVolume);
								denominator = listETH.Sum(eth => eth.QuoteVolume * eth.ChangePercentage) / sumQV;
								numerator = Math.Sqrt(listETH.Sum(eth => eth.QuoteVolume * (eth.ChangePercentage - denominator * denominator)) / sumQV);

								if (!_exchange.volatilityToday.ContainsKey(p.Symbol))
									_exchange.volatilityToday.Add(p.Symbol, numerator / denominator);
								else _exchange.volatilityToday[p.Symbol] = numerator / denominator;

								listETH.Clear();
								break;

							case "BNB-USDT":
								if (p.QuoteVolume != null && p.ChangePercentage != null)
									_dataContext.KucoinVolBNB.Add(new BNB_USDT_Item
									{
										QuoteVolume = (double)p.QuoteVolume,
										ChangePercentage = (double)p.ChangePercentage
									});
								else
									_dataContext.KucoinVolBNB.Add(new BNB_USDT_Item
									{
										QuoteVolume = 0,
										ChangePercentage = 0
									});
								_dataContext.SaveChanges();

								var listBNB = _dataContext.KucoinVolBNB.ToList();

								sumQV = listBNB.Sum(eth => eth.QuoteVolume);
								denominator = listBNB.Sum(eth => eth.QuoteVolume * eth.ChangePercentage) / sumQV;
								numerator = Math.Sqrt(listBNB.Sum(eth => eth.QuoteVolume * (eth.ChangePercentage - denominator * denominator)) / sumQV);

								if (!_exchange.volatilityToday.ContainsKey(p.Symbol))
									_exchange.volatilityToday.Add(p.Symbol, numerator / denominator);
								else _exchange.volatilityToday[p.Symbol] = numerator / denominator;

								listBNB.Clear();
								break;

							case "XRP-USDT":
								if (p.QuoteVolume != null && p.ChangePercentage != null)
									_dataContext.KucoinVolXRP.Add(new XRP_USDT_Item
									{
										QuoteVolume = (double)p.QuoteVolume,
										ChangePercentage = (double)p.ChangePercentage
									});
								else
									_dataContext.KucoinVolXRP.Add(new XRP_USDT_Item
									{
										QuoteVolume = 0,
										ChangePercentage = 0
									});
								_dataContext.SaveChanges();

								var listXRP = _dataContext.KucoinVolXRP.ToList();

								sumQV = listXRP.Sum(eth => eth.QuoteVolume);
								denominator = listXRP.Sum(eth => eth.QuoteVolume * eth.ChangePercentage) / sumQV;
								numerator = Math.Sqrt(listXRP.Sum(eth => eth.QuoteVolume * (eth.ChangePercentage - denominator * denominator)) / sumQV);

								if (!_exchange.volatilityToday.ContainsKey(p.Symbol))
									_exchange.volatilityToday.Add(p.Symbol, numerator / denominator);
								else _exchange.volatilityToday[p.Symbol] = numerator / denominator;

								listXRP.Clear();
								break;

							case "UNI-USDT":
								if (p.QuoteVolume != null && p.ChangePercentage != null)
									_dataContext.KucoinVolUNI.Add(new UNI_USDT_Item
									{
										QuoteVolume = (double)p.QuoteVolume,
										ChangePercentage = (double)p.ChangePercentage
									});
								else
									_dataContext.KucoinVolUNI.Add(new UNI_USDT_Item
									{
										QuoteVolume = 0,
										ChangePercentage = 0
									});
								_dataContext.SaveChanges();

								var listUNI = _dataContext.KucoinVolUNI.ToList();

								sumQV = listUNI.Sum(eth => eth.QuoteVolume);
								denominator = listUNI.Sum(eth => eth.QuoteVolume * eth.ChangePercentage) / sumQV;
								numerator = Math.Sqrt(listUNI.Sum(eth => eth.QuoteVolume * (eth.ChangePercentage - denominator * denominator)) / sumQV);

								if (!_exchange.volatilityToday.ContainsKey(p.Symbol))
									_exchange.volatilityToday.Add(p.Symbol, numerator / denominator);
								else _exchange.volatilityToday[p.Symbol] = numerator / denominator;

								listUNI.Clear();
								break;

							case "LTC-USDT":
								if (p.QuoteVolume != null && p.ChangePercentage != null)
									_dataContext.KucoinVolLTC.Add(new LTC_USDT_Item
									{
										QuoteVolume = (double)p.QuoteVolume,
										ChangePercentage = (double)p.ChangePercentage
									});
								else
									_dataContext.KucoinVolLTC.Add(new LTC_USDT_Item
									{
										QuoteVolume = 0,
										ChangePercentage = 0
									});
								_dataContext.SaveChanges();

								var listLTC = _dataContext.KucoinVolLTC.ToList();

								sumQV = listLTC.Sum(eth => eth.QuoteVolume);
								denominator = listLTC.Sum(eth => eth.QuoteVolume * eth.ChangePercentage) / sumQV;
								numerator = Math.Sqrt(listLTC.Sum(eth => eth.QuoteVolume * (eth.ChangePercentage - denominator * denominator)) / sumQV);

								if (!_exchange.volatilityToday.ContainsKey(p.Symbol))
									_exchange.volatilityToday.Add(p.Symbol, numerator / denominator);
								else _exchange.volatilityToday[p.Symbol] = numerator / denominator;

								listLTC.Clear();
								break;

							case "DOGE-USDT":
								if (p.QuoteVolume != null && p.ChangePercentage != null)
									_dataContext.KucoinVolDOGE.Add(new DOGE_USDT_Item
									{
										QuoteVolume = (double)p.QuoteVolume,
										ChangePercentage = (double)p.ChangePercentage
									});
								else
									_dataContext.KucoinVolDOGE.Add(new DOGE_USDT_Item
									{
										QuoteVolume = 0,
										ChangePercentage = 0
									});
								_dataContext.SaveChanges();

								var listDOGE = _dataContext.KucoinVolDOGE.ToList();

								sumQV = listDOGE.Sum(eth => eth.QuoteVolume);
								denominator = listDOGE.Sum(eth => eth.QuoteVolume * eth.ChangePercentage) / sumQV;
								numerator = Math.Sqrt(listDOGE.Sum(eth => eth.QuoteVolume * (eth.ChangePercentage - denominator * denominator)) / sumQV);

								if (!_exchange.volatilityToday.ContainsKey(p.Symbol))
									_exchange.volatilityToday.Add(p.Symbol, numerator / denominator);
								else _exchange.volatilityToday[p.Symbol] = numerator / denominator;

								listDOGE.Clear();
								break;

							case "MATIC-USDT":
								if (p.QuoteVolume != null && p.ChangePercentage != null)
									_dataContext.KucoinVolMATIC.Add(new MATIC_USDT_Item
									{
										QuoteVolume = (double)p.QuoteVolume,
										ChangePercentage = (double)p.ChangePercentage
									});
								else
									_dataContext.KucoinVolMATIC.Add(new MATIC_USDT_Item
									{
										QuoteVolume = 0,
										ChangePercentage = 0
									});
								_dataContext.SaveChanges();

								var listMATIC = _dataContext.KucoinVolMATIC.ToList();

								sumQV = listMATIC.Sum(eth => eth.QuoteVolume);
								denominator = listMATIC.Sum(eth => eth.QuoteVolume * eth.ChangePercentage) / sumQV;
								numerator = Math.Sqrt(listMATIC.Sum(eth => eth.QuoteVolume * (eth.ChangePercentage - denominator * denominator)) / sumQV);

								if (!_exchange.volatilityToday.ContainsKey(p.Symbol))
									_exchange.volatilityToday.Add(p.Symbol, numerator / denominator);
								else _exchange.volatilityToday[p.Symbol] = numerator / denominator;

								listMATIC.Clear();
								break;

							case "SOL-USDT":
								if (p.QuoteVolume != null && p.ChangePercentage != null)
									_dataContext.KucoinVolSOL.Add(new SOL_USDT_Item
									{
										QuoteVolume = (double)p.QuoteVolume,
										ChangePercentage = (double)p.ChangePercentage
									});
								else
									_dataContext.KucoinVolSOL.Add(new SOL_USDT_Item
									{
										QuoteVolume = 0,
										ChangePercentage = 0
									});
								_dataContext.SaveChanges();

								var listSOL = _dataContext.KucoinVolSOL.ToList();

								sumQV = listSOL.Sum(eth => eth.QuoteVolume);
								denominator = listSOL.Sum(eth => eth.QuoteVolume * eth.ChangePercentage) / sumQV;
								numerator = Math.Sqrt(listSOL.Sum(eth => eth.QuoteVolume * (eth.ChangePercentage - denominator * denominator)) / sumQV);

								if (!_exchange.volatilityToday.ContainsKey(p.Symbol))
									_exchange.volatilityToday.Add(p.Symbol, numerator / denominator);
								else _exchange.volatilityToday[p.Symbol] = numerator / denominator;

								listSOL.Clear();
								break;

							case "ARB-USDT":
								if (p.QuoteVolume != null && p.ChangePercentage != null)
									_dataContext.KucoinVolARB.Add(new ARB_USDT_Item
									{
										QuoteVolume = (double)p.QuoteVolume,
										ChangePercentage = (double)p.ChangePercentage
									});
								else
									_dataContext.KucoinVolARB.Add(new ARB_USDT_Item
									{
										QuoteVolume = 0,
										ChangePercentage = 0
									});
								_dataContext.SaveChanges();

								var listARB = _dataContext.KucoinVolARB.ToList();

								sumQV = listARB.Sum(eth => eth.QuoteVolume);
								denominator = listARB.Sum(eth => eth.QuoteVolume * eth.ChangePercentage) / sumQV;
								numerator = Math.Sqrt(listARB.Sum(eth => eth.QuoteVolume * (eth.ChangePercentage - denominator * denominator)) / sumQV);

								if (!_exchange.volatilityToday.ContainsKey(p.Symbol))
									_exchange.volatilityToday.Add(p.Symbol, numerator / denominator);
								else _exchange.volatilityToday[p.Symbol] = numerator / denominator;

								listARB.Clear();
								break;
						}
					}
				});
				Console.WriteLine("Kucoin Volatility Done");
			}
		}
	}
}
