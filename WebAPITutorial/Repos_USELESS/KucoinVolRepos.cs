using WebAPITutorial.DBContexts;
using WebAPITutorial.Models;
using static WebAPITutorial.Models.KucoinVolatilityEntity;

namespace WebAPITutorial.Repos
{
	public class KucoinVolRepos
	{
		private KucoinVolContext _context;
		public KucoinVolRepos(KucoinVolContext context) 
		{
			_context = context;
		}

		public void Add<T>(T item) where T : KucoinVolatilityEntity
		{
			if (item is BTC_USDT_Item btcItem)
			{
				_context.KucoinVolBTC.Add(btcItem);
			}
			else if (item is ETH_USDT_Item ethItem)
			{
				_context.KucoinVolETH.Add(ethItem);
			}
			else if (item is BNB_USDT_Item bnbItem)
			{
				_context.KucoinVolBNB.Add(bnbItem);
			}
			else if (item is XRP_USDT_Item xrpItem)
			{
				_context.KucoinVolXRP.Add(xrpItem);
			}
			else if (item is UNI_USDT_Item uniItem)
			{
				_context.KucoinVolUNI.Add(uniItem);
			}
			else if (item is LTC_USDT_Item ltcItem)
			{
				_context.KucoinVolLTC.Add(ltcItem);
			}
			else if (item is DOGE_USDT_Item dogeItem)
			{
				_context.KucoinVolDOGE.Add(dogeItem);
			}
			else if (item is MATIC_USDT_Item maticItem)
			{
				_context.KucoinVolMATIC.Add(maticItem);
			}
			else if (item is SOL_USDT_Item solItem)
			{
				_context.KucoinVolSOL.Add(solItem);
			}
			else if (item is ARB_USDT_Item arbItem)
			{
				_context.KucoinVolARB.Add(arbItem);
			}
		}
	}
}
