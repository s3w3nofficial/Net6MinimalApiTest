using MassTransit;
using WebApi.Contracts;

namespace WebApi.Consumers;

public class SubmitOrderConsumer
    : IConsumer<SubmitOrder>
{
    private readonly ILogger<SubmitOrderConsumer> _logger;
    private readonly IPublishEndpoint _publishEndpoint;
    public SubmitOrderConsumer(ILogger<SubmitOrderConsumer> logger, IPublishEndpoint publishEndpoint)
    {
        this._logger = logger;
        this._publishEndpoint = publishEndpoint;
    }
    
    public async Task Consume(ConsumeContext<SubmitOrder> context)
    {
        this._logger.LogInformation($"Order with Id: {context.Message.OrderId} is being proccessed");

        // simulate delay
        await Task.Delay(15000);

        await this._publishEndpoint.Publish<AcceptOrder>(new
        {
            context.Message.OrderId
        });

        this._logger.LogInformation($"Order with Id: {context.Message.OrderId} was successfuly accepted");
    }
}
