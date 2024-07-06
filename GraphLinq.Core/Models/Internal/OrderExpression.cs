using System.Linq.Expressions;

namespace GraphLinq.Core.Models.Internal
{
    public class OrderExpression
    {
        public OrderExpression(LambdaExpression expression, OrderDirection direction)
        {
            Expression = expression;
            Direction = direction;
        }

        public LambdaExpression Expression { get; }
        public OrderDirection Direction { get; }
    }
}
