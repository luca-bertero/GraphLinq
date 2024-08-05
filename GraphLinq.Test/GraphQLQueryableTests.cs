using Microsoft.VisualStudio.TestTools.UnitTesting;
using GraphLinq.Core.GraphQLQueryable;
using GraphLinq.Core.GraphQLQueryable.Abstractions;
using GraphLinq.Tests.TestsInfrastructure;

namespace SmartGraphQLClient.Tests.Core.GraphQLQueryable
{
    [TestClass]
    public partial class GraphQLQueryableTests
    {
        private IGraphQLQueryable<T> CreateQueryable<T>()
            => new GraphQLQueryable<T>();
    }
}
