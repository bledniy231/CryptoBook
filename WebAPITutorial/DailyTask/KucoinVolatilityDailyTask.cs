using Kucoin.Net.Objects.Models.Spot;
using Microsoft.EntityFrameworkCore;
using WebAPITutorial.DBContexts;
using WebAPITutorial.Exchanges;
using WebAPITutorial.Models;
using static WebAPITutorial.Models.KucoinVolatilityEntity;

namespace WebAPITutorial.DailyTask
{
	public class KucoinVolatilityDailyTask : IDailyTask
	{
		private KucoinExchange _exchange;
		private IServiceProvider _serviceProvider;

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

		private void SetVolatilityWeight<T>(KucoinAllTick prod, List<T> list) where T : KucoinVolatilityEntity
		{

			double denominator = list.Sum(c => c.QuoteVolume);
			double numerator = 0;
			double averDevitation = 0;
			list.ForEach(c =>
			{
				double logReturn = Math.Log((prod.LastPrice != null ? (double)prod.LastPrice : 0) / c.Price);
				numerator += logReturn * c.QuoteVolume;
			});
			double weightedAverReturn = numerator / denominator;
			list.ForEach(c =>
			{
				double logReturn = Math.Log((prod.LastPrice != null ? (double)prod.LastPrice : 0) / c.Price);
				double devitation = (logReturn - weightedAverReturn) * (logReturn - weightedAverReturn);
				averDevitation += devitation;
			});
			double vol = Math.Sqrt(averDevitation / list.Count);

			if (!_exchange.volatilityToday.ContainsKey(prod.Symbol))
				_exchange.volatilityToday.Add(prod.Symbol, Double.IsNormal(vol) ? Math.Round(vol, 10) : 0);
			else _exchange.volatilityToday[prod.Symbol] = Double.IsNormal(vol) ? Math.Round(vol, 10) : 0;
		}

		private void SetVolatilityHistorical<T>(KucoinAllTick prod, List<T> list) where T : KucoinVolatilityEntity
		{
			double averDevitation = 0;
			double mean = 0;
			list.ForEach(c =>
			{
				double a = (prod.LastPrice != null ? (double)prod.LastPrice : 0) / c.Price;
				double logReturn = Math.Log(a);
				mean += logReturn;
			});
			mean = mean / list.Count;
			list.ForEach(c =>
			{
				double a = (prod.LastPrice != null ? (double)prod.LastPrice : 0) / c.Price;
				double logReturn = Math.Log(a);
				double devitation = (logReturn - mean) * (logReturn - mean);
				averDevitation += devitation;
			});
			double vol = Math.Sqrt(averDevitation /*/ list.Count*/);

			if (!_exchange.volatilityToday.ContainsKey(prod.Symbol))
				_exchange.volatilityToday.Add(prod.Symbol, Double.IsNormal(vol) ? Math.Round(vol * 100, 10) : 0);
			else _exchange.volatilityToday[prod.Symbol] = Double.IsNormal(vol) ? Math.Round(vol * 100, 10) : 0;
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
						//double denominator, numerator = 0;
						//double weightedAverReturn = 0;
						//double averDevitation = 0;
						byte amountOfRows = 10;
						var _dataContext = scope.ServiceProvider.GetRequiredService<KucoinVolContext>();
						switch (p.Symbol)
						{
							case "BTC-USDT":
								if (p.QuoteVolume != null && p.LastPrice != null)
									_dataContext.KucoinVolBTC.Add(new BTC_USDT_Item
									{
										QuoteVolume = (double)p.QuoteVolume,
										Price = (double)p.LastPrice
									});
								else
									_dataContext.KucoinVolBTC.Add(new BTC_USDT_Item
									{
										QuoteVolume = 0,
										Price = 0
									});
								_dataContext.SaveChanges();

								var listBTC = _dataContext.KucoinVolBTC.ToList().TakeLast(amountOfRows).ToList();

								SetVolatilityWeight(p, listBTC);
								//SetVolatilityHistorical(p, listBTC);

								//sumQV = listBTC.Sum(btc => btc.QuoteVolume);
								//denominator = listBTC.Sum(btc => btc.QuoteVolume * btc.ChangePercentage) / sumQV;
								//numerator = Math.Sqrt(listBTC.Sum(btc => btc.QuoteVolume * (btc.ChangePercentage - denominator * denominator)) / sumQV);

								listBTC.Clear();
								break;

							case "ETH-USDT":
								if (p.QuoteVolume != null && p.LastPrice != null)
									_dataContext.KucoinVolETH.Add(new ETH_USDT_Item
									{
										QuoteVolume = (double)p.QuoteVolume,
										Price = (double)p.LastPrice
									});
								else
									_dataContext.KucoinVolETH.Add(new ETH_USDT_Item
									{
										QuoteVolume = 0,
										Price = 0
									});
								_dataContext.SaveChanges();

								var listETH = _dataContext.KucoinVolETH.ToList().TakeLast(amountOfRows).ToList();

								SetVolatilityWeight(p, listETH);
								//SetVolatilityHistorical(p, listETH);

								//sumQV = listETH.Sum(eth => eth.QuoteVolume);
								//denominator = listETH.Sum(eth => eth.QuoteVolume * eth.ChangePercentage) / sumQV;
								//numerator = Math.Sqrt(listETH.Sum(eth => eth.QuoteVolume * (eth.ChangePercentage - denominator * denominator)) / sumQV);

								listETH.Clear();
								break;

							case "BNB-USDT":
								if (p.QuoteVolume != null && p.LastPrice != null)
									_dataContext.KucoinVolBNB.Add(new BNB_USDT_Item
									{
										QuoteVolume = (double)p.QuoteVolume,
										Price = (double)p.LastPrice
									});
								else
									_dataContext.KucoinVolBNB.Add(new BNB_USDT_Item
									{
										QuoteVolume = 0,
										Price = 0
									});
								_dataContext.SaveChanges();

								var listBNB = _dataContext.KucoinVolBNB.ToList().TakeLast(amountOfRows).ToList();

								SetVolatilityWeight(p, listBNB);
								//SetVolatilityHistorical(p, listBNB);

								//sumQV = listBNB.Sum(eth => eth.QuoteVolume);
								//denominator = listBNB.Sum(eth => eth.QuoteVolume * eth.ChangePercentage) / sumQV;
								//numerator = Math.Sqrt(listBNB.Sum(eth => eth.QuoteVolume * (eth.ChangePercentage - denominator * denominator)) / sumQV);

								listBNB.Clear();
								break;

							case "XRP-USDT":
								if (p.QuoteVolume != null && p.LastPrice != null)
									_dataContext.KucoinVolXRP.Add(new XRP_USDT_Item
									{
										QuoteVolume = (double)p.QuoteVolume,
										Price = (double)p.LastPrice
									});
								else
									_dataContext.KucoinVolXRP.Add(new XRP_USDT_Item
									{
										QuoteVolume = 0,
										Price = 0
									});
								_dataContext.SaveChanges();

								var listXRP = _dataContext.KucoinVolXRP.ToList().TakeLast(amountOfRows).ToList();

								SetVolatilityWeight(p, listXRP);
								//SetVolatilityHistorical(p, listXRP);

								//sumQV = listXRP.Sum(eth => eth.QuoteVolume);
								//denominator = listXRP.Sum(eth => eth.QuoteVolume * eth.ChangePercentage) / sumQV;
								//numerator = Math.Sqrt(listXRP.Sum(eth => eth.QuoteVolume * (eth.ChangePercentage - denominator * denominator)) / sumQV);

								listXRP.Clear();
								break;

							case "UNI-USDT":
								if (p.QuoteVolume != null && p.LastPrice != null)
									_dataContext.KucoinVolUNI.Add(new UNI_USDT_Item
									{
										QuoteVolume = (double)p.QuoteVolume,
										Price = (double)p.LastPrice
									});
								else
									_dataContext.KucoinVolUNI.Add(new UNI_USDT_Item
									{
										QuoteVolume = 0,
										Price = 0
									});
								_dataContext.SaveChanges();

								var listUNI = _dataContext.KucoinVolUNI.ToList().TakeLast(amountOfRows).ToList();

								SetVolatilityWeight(p, listUNI);
								//SetVolatilityHistorical(p, listUNI);

								//sumQV = listUNI.Sum(eth => eth.QuoteVolume);
								//denominator = listUNI.Sum(eth => eth.QuoteVolume * eth.ChangePercentage) / sumQV;
								//numerator = Math.Sqrt(listUNI.Sum(eth => eth.QuoteVolume * (eth.ChangePercentage - denominator * denominator)) / sumQV);

								listUNI.Clear();
								break;

							case "LTC-USDT":
								if (p.QuoteVolume != null && p.LastPrice != null)
									_dataContext.KucoinVolLTC.Add(new LTC_USDT_Item
									{
										QuoteVolume = (double)p.QuoteVolume,
										Price = (double)p.LastPrice
									});
								else
									_dataContext.KucoinVolLTC.Add(new LTC_USDT_Item
									{
										QuoteVolume = 0,
										Price = 0
									});
								_dataContext.SaveChanges();

								var listLTC = _dataContext.KucoinVolLTC.ToList().TakeLast(amountOfRows).ToList();

								SetVolatilityWeight(p, listLTC);
								//SetVolatilityHistorical(p, listLTC);

								//sumQV = listLTC.Sum(eth => eth.QuoteVolume);
								//denominator = listLTC.Sum(eth => eth.QuoteVolume * eth.ChangePercentage) / sumQV;
								//numerator = Math.Sqrt(listLTC.Sum(eth => eth.QuoteVolume * (eth.ChangePercentage - denominator * denominator)) / sumQV);

								listLTC.Clear();
								break;

							case "DOGE-USDT":
								if (p.QuoteVolume != null && p.LastPrice != null)
									_dataContext.KucoinVolDOGE.Add(new DOGE_USDT_Item
									{
										QuoteVolume = (double)p.QuoteVolume,
										Price = (double)p.LastPrice
									});
								else
									_dataContext.KucoinVolDOGE.Add(new DOGE_USDT_Item
									{
										QuoteVolume = 0,
										Price = 0
									});
								_dataContext.SaveChanges();

								var listDOGE = _dataContext.KucoinVolDOGE.ToList().TakeLast(amountOfRows).ToList();

								SetVolatilityWeight(p, listDOGE);
								//SetVolatilityHistorical(p, listDOGE);

								//sumQV = listDOGE.Sum(eth => eth.QuoteVolume);
								//denominator = listDOGE.Sum(eth => eth.QuoteVolume * eth.ChangePercentage) / sumQV;
								//numerator = Math.Sqrt(listDOGE.Sum(eth => eth.QuoteVolume * (eth.ChangePercentage - denominator * denominator)) / sumQV);

								listDOGE.Clear();
								break;

							case "MATIC-USDT":
								if (p.QuoteVolume != null && p.LastPrice != null)
									_dataContext.KucoinVolMATIC.Add(new MATIC_USDT_Item
									{
										QuoteVolume = (double)p.QuoteVolume,
										Price = (double)p.LastPrice
									});
								else
									_dataContext.KucoinVolMATIC.Add(new MATIC_USDT_Item
									{
										QuoteVolume = 0,
										Price = 0
									});
								_dataContext.SaveChanges();

								var listMATIC = _dataContext.KucoinVolMATIC.ToList().TakeLast(amountOfRows).ToList();

								SetVolatilityWeight(p, listMATIC);
								//SetVolatilityHistorical(p, listMATIC);

								//sumQV = listMATIC.Sum(eth => eth.QuoteVolume);
								//denominator = listMATIC.Sum(eth => eth.QuoteVolume * eth.ChangePercentage) / sumQV;
								//numerator = Math.Sqrt(listMATIC.Sum(eth => eth.QuoteVolume * (eth.ChangePercentage - denominator * denominator)) / sumQV);

								listMATIC.Clear();
								break;

							case "SOL-USDT":
								if (p.QuoteVolume != null && p.LastPrice != null)
									_dataContext.KucoinVolSOL.Add(new SOL_USDT_Item
									{
										QuoteVolume = (double)p.QuoteVolume,
										Price = (double)p.LastPrice
									});
								else
									_dataContext.KucoinVolSOL.Add(new SOL_USDT_Item
									{
										QuoteVolume = 0,
										Price = 0
									});
								_dataContext.SaveChanges();

								var listSOL = _dataContext.KucoinVolSOL.ToList().TakeLast(amountOfRows).ToList();

								SetVolatilityWeight(p, listSOL);
								//SetVolatilityHistorical(p, listSOL);

								//sumQV = listSOL.Sum(eth => eth.QuoteVolume);
								//denominator = listSOL.Sum(eth => eth.QuoteVolume * eth.ChangePercentage) / sumQV;
								//numerator = Math.Sqrt(listSOL.Sum(eth => eth.QuoteVolume * (eth.ChangePercentage - denominator * denominator)) / sumQV);

								listSOL.Clear();
								break;

							case "ARB-USDT":
								if (p.QuoteVolume != null && p.LastPrice != null)
									_dataContext.KucoinVolARB.Add(new ARB_USDT_Item
									{
										QuoteVolume = (double)p.QuoteVolume,
										Price = (double)p.LastPrice
									});
								else
									_dataContext.KucoinVolARB.Add(new ARB_USDT_Item
									{
										QuoteVolume = 0,
										Price = 0
									});
								_dataContext.SaveChanges();

								var listARB = _dataContext.KucoinVolARB.ToList().TakeLast(amountOfRows).ToList();

								SetVolatilityWeight(p, listARB);
								//SetVolatilityHistorical(p, listARB);

								//sumQV = listARB.Sum(eth => eth.QuoteVolume);
								//denominator = listARB.Sum(eth => eth.QuoteVolume * eth.ChangePercentage) / sumQV;
								//numerator = Math.Sqrt(listARB.Sum(eth => eth.QuoteVolume * (eth.ChangePercentage - denominator * denominator)) / sumQV);

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
