namespace SenderWorker;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;

    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogWarning("Sending strudtured log from sender worker", new WorkerLogContainer()
            {
                Person = new Person("test", "test", "job test"),
                Grade = new Grade("Good", 15)
            });

            await Task.Delay(1000, stoppingToken);
        }
    }
}