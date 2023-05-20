using WebAPITutorial.DollarCurrency;

namespace WebAPITutorial.DailyTask
{
	public class DollarCurrencyDailyTask : IDailyTask
	{
		private readonly IDollarCurrency dollarCurrency;

		public DollarCurrencyDailyTask(IDollarCurrency dollarCurrency)
		{
			this.dollarCurrency = dollarCurrency;
		}

		public async Task DoTaskAsync()
		{
			decimal dollar = await dollarCurrency.GetDollarCurrencyAsync();
			Console.WriteLine($"Dollar: {dollar}");
			await File.WriteAllTextAsync(
				"DollarCurrency\\ActualDollarCurrency.txt",
				dollar.ToString() + "\n");
		}
	}
}
