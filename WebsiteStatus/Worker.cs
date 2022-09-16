namespace WebsiteStatus;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private HttpClient _client;
    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;
    }

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        _client = new HttpClient();
        return base.StartAsync(cancellationToken);
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        _client.Dispose();
        _logger.LogInformation("The service has been stopped...");
        return base.StopAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var result = await _client.GetAsync("https://ya.ru/");
            if (result.IsSuccessStatusCode)
            {
                _logger.LogInformation("The website is up. Status code {StatusCode}", result.StatusCode);
            }
            else
            {   
                // Send email, text, etc

                _logger.LogError("The website is down. Status code {StatusCode", result.StatusCode);
            }
            await Task.Delay(5000, stoppingToken);
        }
    }
}