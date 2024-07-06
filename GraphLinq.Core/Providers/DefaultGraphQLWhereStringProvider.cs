using GraphLinq.Core.Providers.Abstractions;
using GraphLinq.Core.Visitors.WhereExpressionVisitor;
using System.Linq.Expressions;
using System.Text;

namespace GraphLinq.Core.Providers
{
    internal class DefaultGraphQLWhereStringProvider : IGraphQLWhereStringProvider
    {
        public string? Build(Type entityType, List<LambdaExpression> predicates)
        {
            const string keyword = "where";

            if (predicates.Count == 0) return null;

            if (predicates.Count == 1)
            {
                return $"{keyword}: {{\n{BuildString(predicates[0])}\n}}";
            }

            var sb = new StringBuilder();
            sb.AppendLine($"{keyword}: {{");
            sb.AppendLine("and: [ ");
            foreach (var predicate in predicates)
            {
                sb.Append(" { ");
                sb.Append(BuildString(predicate));
                sb.AppendLine(" } ");
            }
            sb.AppendLine(" ]");
            sb.AppendLine(" }");

            return sb.ToString();
        }

        private string BuildString(LambdaExpression predicate)
        {
            var visitor = new WhereExpressionVisitor(predicate);
            visitor.Visit();
            return visitor.ToString();
        }
    }
}
