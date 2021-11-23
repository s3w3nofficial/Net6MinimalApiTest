using MassTransit;
using WebApi.Contracts;
using WebApi.Models;
using WebApi.RequestModels;

namespace WebApi.EndpointDefinitions;

public class OrderEndpointDefinition : IEndpointDefinition
{
    public void DefineEndpoints(WebApplication app)
    {
        app.MapGet("/{orderId}", async (Guid orderId, IRequestClient<GetOrder> getOrderClient, CancellationToken cancletionToken) =>
        {
            Response response = await getOrderClient.GetResponse<Order, OrderNotFound>(new
            {
                orderId
            }, cancletionToken);

            return response switch
            {
                (_, Order x) => Results.Json(new OrderModel
                {
                    OrderId = x.OrderId,
                    OrderNumber = x.OrderNumber,
                    Status = x.Status
                }),
                (_, OrderNotFound x) => Results.Json(new OrderNotFoundModel
                {
                    OrderId = x.OrderId
                }, statusCode: 404),
                _ => Results.Json(new OrderModel
                {
                    OrderId = orderId
                })
            };
        });

        app.MapPost("/submit", async (IPublishEndpoint publishEndpoint, SubmitOrderModel orderModel, CancellationToken cancellationToken) =>
        {
            await publishEndpoint.Publish<SubmitOrder>(new
            {
                orderModel.OrderId,
                orderModel.OrderNumber
            }, cancellationToken);

            Results.Json(new OrderModel
            {
                OrderId = orderModel.OrderId,
                OrderNumber = orderModel.OrderNumber,
                Status = "Submited"
            }, statusCode: 202);
        });
    }

    public void DefineServices(IServiceCollection services)
    {

    }
}
