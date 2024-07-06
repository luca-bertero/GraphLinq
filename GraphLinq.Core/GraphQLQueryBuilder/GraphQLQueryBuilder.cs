using GraphLinq.Core.GraphQLQueryBuilder.Models;
using GraphLinq.Core.Models.Constants;
using GraphLinq.Core.Models.Internal;
using GraphLinq.Core.Providers.Abstractions;
using System.Linq.Expressions;

namespace GraphLinq.Core.GraphQLQueryBuilder
{
    internal class GraphQLQueryBuilder
    {
        private readonly GraphQLQueryBuilderConfiguration _configuration;
        private readonly IGraphQLEndpointProvider _endpointProvider;
        private readonly IGraphQLWhereStringProvider _whereStringProvider;
        private readonly IGraphQLOrderStringProvider _orderStringProvider;
        private readonly IGraphQLBodyStringProvider _bodyStringProvider;
        private readonly IGraphQLOffsetPaginationStringProvider _offsetPaginationStringProvider;
        private readonly IGraphQLValueFormatProvider _valueFormatProvider;

        public GraphQLQueryBuilder(
            GraphQLQueryBuilderConfiguration config, 
            IGraphQLEndpointProvider endpointProvider, 
            IGraphQLWhereStringProvider whereStringProvider, 
            IGraphQLOrderStringProvider orderStringProvider, 
            IGraphQLBodyStringProvider bodyStringProvider, 
            IGraphQLOffsetPaginationStringProvider offsetPaginationStringProvider, 
            IGraphQLValueFormatProvider valueFormatProvider)
        {
            _configuration = config;
            _endpointProvider = endpointProvider;
            _whereStringProvider = whereStringProvider;
            _orderStringProvider = orderStringProvider;
            _bodyStringProvider = bodyStringProvider;
            _offsetPaginationStringProvider = offsetPaginationStringProvider;
            _valueFormatProvider = valueFormatProvider;
        }

        public string Build(out GraphQLRequestMetadataConfiguration config)
        {
            var endpoint = GetEndpoint();

            var parameters = BuildArguments();
            var body = BuildBody(out var rootSelector);

            config = new(_configuration.RootEntityType, rootSelector, _configuration.CallChain);

            return @$"
                {{ 
                    {endpoint} {(parameters.Count > 0 ? $"( {string.Join("\n", parameters)} ) " : "")} 
                    {{
                    {body}
                    }}
                }}";
        }

        internal void ConfigureRequestOptions(Action<GraphQLRequestConfiguration> configure)
        {
            _configuration.RequestConfiguration.ClearInternal();
            configure(_configuration.RequestConfiguration);
        }

        private string GetEndpoint()
        {
            var endpoint = _configuration.RequestConfiguration.Endpoint;
            if (endpoint is null)
            {
                endpoint = _endpointProvider.GetGraphQLEndpoint(_configuration.RootEntityType);
            }

            return endpoint;
        }

        private List<string> BuildArguments()
        {
            var arguments = new List<string>();

            var whereArguments = BuildWhereString();
            if (whereArguments is not null) arguments.Add(whereArguments);

            var orderArgument = BuildOrderString();
            if (orderArgument is not null) arguments.Add(orderArgument);

            var skipArgument = BuildSkipString();
            if (skipArgument is not null) arguments.Add(skipArgument);

            var takeArgument = BuildTakeString();
            if (takeArgument is not null) arguments.Add(takeArgument);

            EnsureArguments(arguments);

            return arguments;
        }

        private string? BuildWhereString()
        {
            return _whereStringProvider.Build(_configuration.RootEntityType, _configuration.CallChain.QueryConditions);
        }

        private string? BuildOrderString()
        {
            return _orderStringProvider.Build(_configuration.RootEntityType, _configuration.CallChain.QueryOrders);
        }

        private string BuildBody(out LambdaExpression rootSelector)
        {
            (var body, rootSelector) = _bodyStringProvider.Build(
                _configuration.RootEntityType,
                _configuration.CallChain.QuerySelector,
                _configuration.CallChain.QueryIncludes,
                new()
                {
                    DisabledIgnoreAttributes = (bool)_configuration.QueryableConfiguration
                        .GetValueOrDefault(QueryableConfigurationKeys.DisabledIgnoreAttributes, false)!
                });
            _configuration.CallChain.ChangeQuerySelector(rootSelector);

            /* CollectionPage */
            if (_configuration.RequestConfiguration.IsCollectionPage)
            {
                const string keyword = "items";
                const string pageInfo = "pageInfo { hasNextPage hasPreviousPage }";
                const string totalCount = "totalCount";

                return $"{keyword} {{\n{body}\n}}\n{pageInfo}\n{totalCount}";
            }
            /* One */
            else if (_configuration.RequestConfiguration.IsFirstOrDefault)
            {
                return body;
            }
            /* Array */
            else if (_configuration.RequestConfiguration.IsArray)
            {
                return body;
            }
            else
            {
                return body;
            }
        }

        private string? BuildSkipString()
        {
            return _offsetPaginationStringProvider.BuildSkip(_configuration.CallChain.QuerySkip);
        }

        private string? BuildTakeString()
        {
            return _offsetPaginationStringProvider.BuildTake(_configuration.CallChain.QueryTake);
        }

        private void EnsureArguments(List<string> arguments)
        {
            if (!_configuration.CallChain.QueryArguments.Any())
            {
                return;
            }

            foreach (var arg in _configuration.CallChain.QueryArguments)
            {
                arguments.Add($"{arg.Key}: {_valueFormatProvider.GetFormattedValue(arg.Value)}");
            }
        }
    }
}
