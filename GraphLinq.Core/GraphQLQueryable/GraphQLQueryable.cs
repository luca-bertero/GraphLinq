using GraphLinq.Core.GraphQLQueryable.Abstractions;
using System.Linq.Expressions;
using GraphLinq.Core.Models.Internal;
using GraphLinq.Core.GraphQLQueryable.Internal;
using GraphLinq.Core.GraphQLQueryBuilder.Models;
using GraphLinq.Core.Providers;

namespace GraphLinq.Core.GraphQLQueryable
{
    internal class GraphQLQueryable<TEntity> :
        IGraphQLQueryable<TEntity>
    {
        public Type GraphQLClientType { get; }
        internal string? Endpoint { get; private set; }
        internal Type RootEntityType { get; private set; }
        internal GraphQLExpressionCallChainConfiguration CallChain { get; private set; } = new();
        internal Dictionary<string, object?> Configuration { get; private set; } = new();

        public GraphQLQueryable(
            Type graphQLClientType,
            string? endpoint = null)
        {
            GraphQLClientType = graphQLClientType;
            RootEntityType = typeof(TEntity);
        }

        internal GraphQLQueryable(
            GraphQLQueryable<TEntity> queryable)
            : this(
                  queryable.GraphQLClientType,
                  queryable.CallChain,
                  queryable.Configuration)
        {
        }

        private GraphQLQueryable(
            Type gpaphQLClientType,
            GraphQLExpressionCallChainConfiguration callChain,
            Dictionary<string, object?> configuration)
            : this(gpaphQLClientType)
        {
            CallChain = callChain;
            Configuration = configuration;
        }

        public string DebugView
        {
            get
            {
                var queryBuilder = GetQueryBuilder();
                return queryBuilder.Build(out var _);
            }
        }

        public string BuildQuery()
        {
            var queryBuilder = GetQueryBuilder();
            return queryBuilder.Build(out var _);
        }

        protected IGraphQLQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate)
        {
            var clone = Clone();
            clone.CallChain.AddCondition(predicate);
            return clone;
        }

        protected IGraphQLQueryable<TOutEntity> Select<TOutEntity>(Expression<Func<TEntity, TOutEntity>> selector)
        {
            var clone = Clone<TOutEntity>();
            clone.CallChain.AddSelector(selector);
            return clone;
        }

        protected IIncludableGraphQLQueryable<TEntity, TProperty> Include<TProperty>(Expression<Func<TEntity, TProperty>> includeExpression)
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

        protected IOrderableGraphQLQueryable<TEntity, TProperty> OrderBy<TProperty>(Expression<Func<TEntity, TProperty>> orderByExpression)
            => OrderByDirection(orderByExpression, OrderDirection.ASC);

        protected IOrderableGraphQLQueryable<TEntity, TProperty> OrderByDescending<TProperty>(Expression<Func<TEntity, TProperty>> orderByExpression)
            => OrderByDirection(orderByExpression, OrderDirection.DESC);

        private IOrderableGraphQLQueryable<TEntity, TProperty> OrderByDirection<TProperty>(Expression<Func<TEntity, TProperty>> expression, OrderDirection direction)
        {
            var clone = Clone();
            clone.CallChain.AddOrder(new OrderExpression(expression, direction));
            return new OrderedGraphQLQueryable<TEntity, TProperty>(clone);
        }

        protected IGraphQLQueryable<TEntity> Skip(int count)
        {
            var clone = Clone();
            clone.CallChain.AddSkip(count);
            return clone;
        }

        protected IGraphQLQueryable<TEntity> Take(int count)
        {
            var clone = Clone();
            clone.CallChain.AddTake(count);
            return clone;
        }

        protected IGraphQLQueryable<TEntity> Argument(string key, object? value)
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
            if (Endpoint is not null && config?.Endpoint is null)
            {
                config ??= new GraphQLRequestConfiguration();
                config.Endpoint = Endpoint;
            }

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
                GraphQLClientType,
                CallChain.Clone(),
                new Dictionary<string, object?>(Configuration))
            {
                RootEntityType = this.RootEntityType
            };

            return clone;
        }
    }
}
