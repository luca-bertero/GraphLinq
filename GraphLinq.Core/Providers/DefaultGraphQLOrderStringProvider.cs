using GraphLinq.Core.Models.Internal;
using GraphLinq.Core.Providers.Abstractions;
using GraphLinq.Core.Visitors.OrderExpressionVisitor;
using System.Linq.Expressions;
using System.Text;

namespace GraphLinq.Core.Providers
{
    internal class DefaultGraphQLOrderStringProvider : IGraphQLOrderStringProvider
    {
        public string? Build(Type entityType, List<OrderExpression> orders)
        {
            const string keyword = "order";

            if (orders.Count == 0) return null;

            var sb = new StringBuilder();
            sb.AppendLine($"{keyword}: [");
            foreach (var order in orders)
            {
                sb.AppendLine($"{{ {BuildString(order.Expression, order.Direction)} }}");
            }
            sb.AppendLine(" ]");

            return sb.ToString();
        }

        private string BuildString(LambdaExpression expression, OrderDirection direction)
        {
            var visitor = new OrderExpressionVisitor(expression, direction);
            visitor.Visit();
            return visitor.ToString();
        }
    }
}
