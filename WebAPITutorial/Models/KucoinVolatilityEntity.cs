using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPITutorial.Models
{
	public abstract class KucoinVolatilityEntity
	{
		[Key]
		public long KucoinVolId { get; set; }
		//public abstract string Symbol { get; set; }
		public bool IsArchived { get; set; } = false;
		public DateTime Date { get; set; } = DateTime.Now;
		[Precision(18, 10)]
		public double QuoteVolume { get; set; }
		[Precision(18, 10)]
		public double ChangePercentage { get; set; }

		[Table(name: "BTC_USDT_Items")]
		public class BTC_USDT_Item : KucoinVolatilityEntity { }

		[Table(name: "ETH_USDT_Items")]
		public class ETH_USDT_Item : KucoinVolatilityEntity { }

		[Table(name: "BNB_USDT_Items")]
		public class BNB_USDT_Item : KucoinVolatilityEntity { }

		[Table(name: "XRP_USDT_Items")]
		public class XRP_USDT_Item : KucoinVolatilityEntity { }

		[Table(name: "UNI_USDT_Items")]
		public class UNI_USDT_Item : KucoinVolatilityEntity { }

		[Table(name: "LTC_USDT_Items")]
		public class LTC_USDT_Item : KucoinVolatilityEntity { }

		[Table(name: "DOGE_USDT_Items")]
		public class DOGE_USDT_Item : KucoinVolatilityEntity { }

		[Table(name: "MATIC_USDT_Items")]
		public class MATIC_USDT_Item : KucoinVolatilityEntity { }

		[Table(name: "SOL_USDT_Items")]
		public class SOL_USDT_Item : KucoinVolatilityEntity { }

		[Table(name: "ARB_USDT_Items")]
		public class ARB_USDT_Item : KucoinVolatilityEntity { }
	}
}
