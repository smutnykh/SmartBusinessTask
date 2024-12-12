using System.Threading.Channels;

namespace ProductionApi.Services
{
	public class BackgroundProcessingService : BackgroundService
	{
		private readonly Channel<Func<CancellationToken, Task>> _queue;
		private readonly ILogger<BackgroundProcessingService> _logger;

		public BackgroundProcessingService(ILogger<BackgroundProcessingService> logger)
		{
			_queue = Channel.CreateUnbounded<Func<CancellationToken, Task>>();
			_logger = logger;
		}

		public void QueueBackgroundWorkItem(Func<CancellationToken, Task> workItem)
		{
			if (workItem == null) throw new ArgumentNullException(nameof(workItem));
			_queue.Writer.TryWrite(workItem);
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			_logger.LogInformation("Background Service is starting.");

			while (!stoppingToken.IsCancellationRequested)
			{
				try
				{
					var workItem = await _queue.Reader.ReadAsync(stoppingToken);
					await workItem(stoppingToken);
				}
				catch (Exception ex)
				{
					_logger.LogError(ex, "Error occurred executing task.");
				}
			}

			_logger.LogInformation("Background Service is stopping.");
		}
	}
}
