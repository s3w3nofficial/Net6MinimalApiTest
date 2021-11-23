using System.Runtime.CompilerServices;
using MassTransit;
using MassTransit.Topology.Topologies;

namespace WebApi.Contracts;

public record AcceptOrder
{
    public Guid OrderId { get; init; }


    [ModuleInitializer]
    internal static void Init()
    {
        GlobalTopology.Send.UseCorrelationId<AcceptOrder>(x => x.OrderId);
    }
}