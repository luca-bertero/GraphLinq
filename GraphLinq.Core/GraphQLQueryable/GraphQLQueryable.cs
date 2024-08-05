using GraphLinq.Core.GraphQLQueryable.Abstractions;
using System.Linq.Expressions;
using GraphLinq.Core.Models.Internal;
using GraphLinq.Core.GraphQLQueryable.Internal;
using GraphLinq.Core.GraphQLQueryBuilder.Models;
using GraphLinq.Core.Providers;

namespace GraphLinq.Core.GraphQLQueryable
{
    public class GraphQLQueryable<TEntity> :
        IGraphQLQueryable<TEntity>
    {
        internal Type RootEntityType { get; private set; }
        internal GraphQLExpressionCallChainConfiguration CallChain { get; private set; } = new();
        internal Dictionary<string, object?> Configuration { get; private set; } = new();

        public GraphQLQueryable()
        {
            RootEntityType = typeof(TEntity);
        }

        internal GraphQLQueryable(
            GraphQLQueryable<TEntity> queryable)
            : this(
                  queryable.CallChain,
                  queryable.Configuration)
        {
        }

        private GraphQLQueryable(
            GraphQLExpressionCallChainConfiguration callChain,
            Dictionary<string, object?> configuration)
        {
            CallChain = callChain;
            Configuration = configuration;
        }

        public string BuildQuery()
        {
            var queryBuilder = GetQueryBuilder();
            return queryBuilder.Build(out var _);
        }

        public IGraphQLQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate)
        {
            var clone = Clone();
            clone.CallChain.AddCondition(predicate);
            return clone;
        }

        public IGraphQLQueryable<TOutEntity> Select<TOutEntity>(Expression<Func<TEntity, TOutEntity>> selector)
        {
            var clone = Clone<TOutEntity>();
            clone.CallChain.AddSelector(selector);
            return clone;
        }

        public IIncludableGraphQLQueryable<TEntity, TProperty> Include<TProperty>(Expression<Func<TEntity, TProperty>> includeExpression)
        {
            var clone = Clone();
            var includeNode = clone.CallChain.QueryIncludes.FirstOrDefault(x => x.RootExpression == includeExpression);
            if (includeNode is null)
            {
                includeNode = new IncludeExpressionNode(includeExpression);
                clone.CallChain.AddInclude(includeNode);
            }

            return new IncludableGraphQLQueryable<TEntity, TProperty>(clone, includeNode);
        }

        public IOrderableGraphQLQueryable<TEntity, TProperty> OrderBy<TProperty>(Expression<Func<TEntity, TProperty>> orderByExpression)
            => OrderByDirection(orderByExpression, OrderDirection.ASC);

        public IOrderableGraphQLQueryable<TEntity, TProperty> OrderByDescending<TProperty>(Expression<Func<TEntity, TProperty>> orderByExpression)
            => OrderByDirection(orderByExpression, OrderDirection.DESC);

        private IOrderableGraphQLQueryable<TEntity, TProperty> OrderByDirection<TProperty>(Expression<Func<TEntity, TProperty>> expression, OrderDirection direction)
        {
            var clone = Clone();
            clone.CallChain.AddOrder(new OrderExpression(expression, direction));
            return new OrderedGraphQLQueryable<TEntity, TProperty>(clone);
        }

        public IGraphQLQueryable<TEntity> Skip(int count)
        {
            var clone = Clone();
            clone.CallChain.AddSkip(count);
            return clone;
        }

        public IGraphQLQueryable<TEntity> Take(int count)
        {
            var clone = Clone();
            clone.CallChain.AddTake(count);
            return clone;
        }

        public IGraphQLQueryable<TEntity> Argument(string key, object? value)
        {
            var clone = Clone();
            clone.CallChain.AddArgument(new(key, value));
            return clone;
        }
        
        protected IGraphQLQueryable<TEntity> Configure(string key, object? value)
        {
            var clone = Clone();
            clone.Configuration[key] = value;
            return clone;
        }

        private GraphQLQueryBuilder.GraphQLQueryBuilder GetQueryBuilder(
            GraphQLRequestConfiguration? config = null)
        {
            return new GraphQLQueryBuilder.GraphQLQueryBuilder(
                new GraphQLQueryBuilderConfiguration(
                    RootEntityType,
                    CallChain,
                    Configuration,
                    config),
                new DefaultGraphQLEndpointProvider(),
                new DefaultGraphQLWhereStringProvider(),
                new DefaultGraphQLOrderStringProvider(),
                new DefaultGraphQLBodyStringProvider(),
                new DefaultGraphQLOffsetPaginationStringProvider(),
                new DefaultGraphQLValueFormatProvider()
            );
        }

        internal GraphQLQueryable<TEntity> Clone()
            => Clone<TEntity>();

        internal GraphQLQueryable<TOutEntity> Clone<TOutEntity>()
        {
            var clone = new GraphQLQueryable<TOutEntity>(
                CallChain.Clone(),
                new Dictionary<string, object?>(Configuration))
            {
                RootEntityType = this.RootEntityType
            };

            return clone;
        }
    }
}
