namespace WebApi.RequestModels;

public record SubmitOrderModel
{
    public Guid OrderId { get; init; }
    public string? OrderNumber { get; init; }
}