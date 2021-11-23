using MassTransit;
using MassTransit.Saga;
using WebApi;
using WebApi.Consumers;
using WebApi.Contracts;
using WebApi.StateMachines;
using IEndpointDefinition = WebApi.IEndpointDefinition;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointDefinitions(typeof(IEndpointDefinition));

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<SubmitOrderConsumer>();

    // x.AddConsumer<MessageConsumer>();

    x.AddSagaStateMachine<OrderStateMachine, OrderState>()
        .InMemoryRepository();

    x.AddRequestClient<GetOrder>();

    x.UsingInMemory((context, cfg) =>
    {
        cfg.ConfigureEndpoints(context);
    });

});
builder.Services.AddMassTransitHostedService(true);

// builder.Services.AddHostedService<Worker>();

var app = builder.Build();
app.UseEndpointDefinitions();

app.Run();