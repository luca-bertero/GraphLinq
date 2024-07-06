using GraphLinq.Core.Models.Internal;
using GraphLinq.Core.Visitors.SelectExpressionVisitor.Models;
using System.Linq.Expressions;

namespace GraphLinq.Core.Providers.Abstractions
{
    public interface IGraphQLBodyStringProvider
    {
        (string Body, LambdaExpression Selector) Build(
            Type entityType, 
            LambdaExpression? selector, 
            List<IncludeExpressionNode> includes,
            SelectExpressionVisitorConfiguration configuration);
    }
}
