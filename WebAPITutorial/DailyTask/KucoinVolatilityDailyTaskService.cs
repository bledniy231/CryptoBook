namespace WebAPITutorial.DailyTask
{
	public class KucoinVolatilityDailyTaskService : BackgroundService
	{
		private readonly KucoinVolatilityDailyTask _task;
		private readonly TimeSpan interval = TimeSpan.FromDays(1);
		private DateTime currentTime;

		public KucoinVolatilityDailyTaskService(KucoinVolatilityDailyTask task) 
		{
			_task = task;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			currentTime = DateTime.Now;
			DateTime startTime = currentTime.Date.AddHours(23); // 00:00
			DateTime endTime = currentTime.Date.AddHours(24); // 01:00

			if (currentTime >= startTime && currentTime < endTime)
			{
				await _task.DoTaskAsync();
				Console.WriteLine("Vol have gotten successfully");
				await Task.Delay(interval, stoppingToken);
			}
			else
			{
				TimeSpan delay = startTime.Add(interval) - currentTime;
				await Task.Delay(delay, stoppingToken);
				await _task.DoTaskAsync();
			}
		}
	}
}
