using Microsoft.EntityFrameworkCore;
using static WebAPITutorial.Models.KucoinVolatilityEntity;

namespace WebAPITutorial.DBContexts
{
	public class KucoinVolContext : DbContext
	{
		public KucoinVolContext(DbContextOptions<KucoinVolContext> options) : base(options) { }

		public DbSet<BTC_USDT_Item> KucoinVolBTC { get; set; }
		public DbSet<ETH_USDT_Item> KucoinVolETH { get; set; }
		public DbSet<BNB_USDT_Item> KucoinVolBNB { get; set; }
		public DbSet<XRP_USDT_Item> KucoinVolXRP { get; set; }
		public DbSet<UNI_USDT_Item> KucoinVolUNI { get; set; }
		public DbSet<LTC_USDT_Item> KucoinVolLTC { get; set; }
		public DbSet<DOGE_USDT_Item> KucoinVolDOGE { get; set; }
		public DbSet<MATIC_USDT_Item> KucoinVolMATIC { get; set; }
		public DbSet<SOL_USDT_Item> KucoinVolSOL { get; set; }
		public DbSet<ARB_USDT_Item> KucoinVolARB { get; set; }


		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			//modelBuilder.Entity<KucoinVolatilityEntity>().HasKey(m => m.KucoinVolId);
			//modelBuilder.Entity<BTC_USDT_Item>().ToTable("BTC_USDT_Items");
			//modelBuilder.Entity<ETH_USDT_Item>().ToTable("ETH_USDT_Items");
			//modelBuilder.Entity<BNB_USDT_Item>().ToTable("BNB_USDT_Items");
			//modelBuilder.Entity<XRP_USDT_Item>().ToTable("XRP_USDT_Items");
			//modelBuilder.Entity<UNI_USDT_Item>().ToTable("UNI_USDT_Items");
			//modelBuilder.Entity<LTC_USDT_Item>().ToTable("LTC_USDT_Items");
			//modelBuilder.Entity<DOGE_USDT_Item>().ToTable("DOGE_USDT_Items");
			//modelBuilder.Entity<MATIC_USDT_Item>().ToTable("MATIC_USDT_Items");
			//modelBuilder.Entity<SOL_USDT_Item>().ToTable("SOL_USDT_Items");
			//modelBuilder.Entity<ARB_USDT_Item>().ToTable("ARB_USDT_Items");
		}
	}
}
