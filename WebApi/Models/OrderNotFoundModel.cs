namespace WebApi.Models;

public record OrderNotFoundModel
{
    public Guid OrderId { get; init; }
}