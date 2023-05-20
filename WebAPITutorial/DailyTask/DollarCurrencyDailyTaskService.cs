namespace WebAPITutorial.DailyTask
{
	public class DollarCurrencyDailyTaskService : BackgroundService
	{
		private readonly DollarCurrencyDailyTask _task;
		private readonly TimeSpan interval = TimeSpan.FromDays(1);
		private DateTime lastRunTime;
		private bool isFirstTime = true;
		private string path = "DollarCurrency\\ActualDollarCurrency.txt";
		
		public DollarCurrencyDailyTaskService(DollarCurrencyDailyTask task)
		{
			_task = task;
			if (File.Exists(path))
			{
				string? dtstr = File.ReadLines(path).ElementAtOrDefault(1);

				if (dtstr != null && DateTime.TryParse(dtstr, out lastRunTime))
				{
					Console.WriteLine($"Read time success: {lastRunTime}");
				}
			}
			else
			{
				lastRunTime = DateTime.Now.AddDays(-1);
				Console.WriteLine("Read time failed, time initialized to last day");
			}
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			while (!stoppingToken.IsCancellationRequested) 
			{
				if (isFirstTime)
				{
					isFirstTime = false;
				}
				else
				{
					TimeSpan delay = GetDelayUntilNextRun();
					await Task.Delay(delay, stoppingToken);
				}

				if (DateTime.Now - lastRunTime >= interval)
				{
					lastRunTime = DateTime.Now;
					await _task.DoTaskAsync();
					await File.AppendAllTextAsync(path, lastRunTime.ToString());
				}
			}
		}

		private TimeSpan GetDelayUntilNextRun()
		{
			DateTime now = DateTime.Now;
			DateTime nextRunTime = lastRunTime.Add(interval);
			if (nextRunTime < now)
			{
				nextRunTime = nextRunTime.Add(interval);
			}
			return nextRunTime - now;
		}
	}
}
