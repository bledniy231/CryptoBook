namespace WebAPITutorial.DailyTask
{
	public class DailyTasksService : BackgroundService
	{
		private readonly IEnumerable<IDailyTask> _tasks;
		private readonly TimeSpan interval = TimeSpan.FromDays(1);
		private DateTime lastRunTime;
		private bool isFirstTime = true;
		private string path = "DollarCurrency\\ActualDollarCurrency.txt";
		
		public DailyTasksService(IEnumerable<IDailyTask> tasks)
		{
			_tasks = tasks;
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
					_tasks.FirstOrDefault(t => t.GetType()
						.Equals(typeof(KucoinVolatilityDailyTask)))?
						.DoTaskAsync();
				}
				else
				{
					TimeSpan delay = GetDelayUntilNextRun();
					await Task.Delay(delay, stoppingToken);
				}

				if (DateTime.Now - lastRunTime >= interval)
				{
					lastRunTime = DateTime.Now;
					foreach (var _task in _tasks)
						await _task.DoTaskAsync();
					await File.AppendAllTextAsync(path, lastRunTime.ToString(), stoppingToken);
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
