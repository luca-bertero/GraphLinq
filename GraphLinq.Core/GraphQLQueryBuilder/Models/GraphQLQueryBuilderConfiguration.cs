using GraphLinq.Core.Models.Internal;

namespace GraphLinq.Core.GraphQLQueryBuilder.Models
{
    internal class GraphQLQueryBuilderConfiguration
    {
        public GraphQLQueryBuilderConfiguration(
            Type rootEntityType,
            GraphQLExpressionCallChainConfiguration callChain,
            IReadOnlyDictionary<string, object?> configuration,
            GraphQLRequestConfiguration? requestConfiguration)
        {
            RootEntityType = rootEntityType;
            CallChain = callChain;
            QueryableConfiguration = configuration;
            RequestConfiguration = requestConfiguration ?? new();
        }

        public Type RootEntityType { get; }
        public GraphQLExpressionCallChainConfiguration CallChain { get; }
        public IReadOnlyDictionary<string, object?> QueryableConfiguration { get; } = new Dictionary<string, object?>();
        public GraphQLRequestConfiguration RequestConfiguration { get; }
    }
}
