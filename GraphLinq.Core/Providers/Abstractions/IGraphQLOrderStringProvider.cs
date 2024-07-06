using GraphLinq.Core.Models.Internal;

namespace GraphLinq.Core.Providers.Abstractions
{
    public interface IGraphQLOrderStringProvider
    {
        string? Build(Type entityType, List<OrderExpression> orders);
    }
}
