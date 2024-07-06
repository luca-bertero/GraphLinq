using GraphLinq.Core.Models.Internal;
using GraphLinq.Core.Providers.Abstractions;
using GraphLinq.Core.Visitors.SelectExpressionVisitor;
using GraphLinq.Core.Visitors.SelectExpressionVisitor.Abstractions;
using GraphLinq.Core.Visitors.SelectExpressionVisitor.Models;
using System.Linq.Expressions;

namespace GraphLinq.Core.Providers
{
    internal class DefaultGraphQLBodyStringProvider : IGraphQLBodyStringProvider
    {
        public (string Body, LambdaExpression Selector) Build(
            Type entityType, 
            LambdaExpression? selector, 
            List<IncludeExpressionNode> includes,
            SelectExpressionVisitorConfiguration configuration)
        {
            SelectExpressionVisitorBase visitor;

            if (selector is not null)
            {
                visitor = new SelectExpressionVisitor(selector, configuration);
            }
            else
            {
                visitor = new DefaultSelectExpressionVisitor(entityType, configuration);
            }

            visitor.Visit();
            visitor.VisitIncludeExpressions(includes);
            return (visitor.ToString(), visitor.GetSelector());
        }
    }
}
