using System.Linq.Expressions;

namespace GraphLinq.Core.Providers.Abstractions
{
    public interface IGraphQLWhereStringProvider
    {
        string? Build(Type entityType, List<LambdaExpression> predicates);
    }
}
