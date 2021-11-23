using MassTransit;
using WebApi.Contracts;

namespace WebApi.Services;

public class Worker : BackgroundService
{
    private readonly IBus _bus;
    private readonly ILogger<Worker> _logger;

    public Worker(IBus bus, ILogger<Worker> logger)
    {
        this._bus = bus;
        this._logger = logger;
    }

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        this._logger.LogInformation("starting worker service");
        return base.StartAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var message = new Message {Text = $"The time is {DateTimeOffset.Now}"};
            // this._logger.LogInformation($"publishing message with text: {message.Text}");
            await _bus.Publish(message);

            await Task.Delay(1000, stoppingToken);
        }
    }
}