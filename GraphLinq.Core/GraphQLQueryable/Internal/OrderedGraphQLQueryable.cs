using GraphLinq.Core.GraphQLQueryable.Abstractions;
using GraphLinq.Core.Models.Internal;
using System.Linq.Expressions;

namespace GraphLinq.Core.GraphQLQueryable.Internal
{
    internal class OrderedGraphQLQueryable<TEntity, TProperty> :
        GraphQLQueryable<TEntity>,
        IOrderableGraphQLQueryable<TEntity, TProperty>
    {
        private readonly GraphQLQueryable<TEntity> Queryable;

        public OrderedGraphQLQueryable(
            GraphQLQueryable<TEntity> queryable)
            : base(queryable)
        {
            Queryable = queryable;
        }

        protected IOrderableGraphQLQueryable<TEntity, TNextProperty> ThenBy<TNextProperty>(
            Expression<Func<TEntity, TNextProperty>> thenByExpression)
            => ThenByDirection(thenByExpression, OrderDirection.ASC);

        protected IOrderableGraphQLQueryable<TEntity, TNextProperty> ThenByDescending<TNextProperty>(
            Expression<Func<TEntity, TNextProperty>> thenByExpression)
            => ThenByDirection(thenByExpression, OrderDirection.DESC);

        private IOrderableGraphQLQueryable<TEntity, TNextProperty> ThenByDirection<TNextProperty>(
            Expression<Func<TEntity, TNextProperty>> expression, OrderDirection direction)
        {
            var queryableClone = Queryable.Clone<TEntity>();
            queryableClone.CallChain.AddOrder(new(expression, direction));
            return new OrderedGraphQLQueryable<TEntity, TNextProperty>(queryableClone);
        }
    }
}