using GraphLinq.Core.GraphQLQueryable.Abstractions;
using GraphLinq.Core.GraphQLQueryable.Utils;

namespace GraphLinq.Extensions
{
    public static partial class IGraphQLQueryableInternalExtensions
    {
        public static IGraphQLQueryable<TEntity> DisableIgnoreAttributes<TEntity>(
            this IGraphQLQueryable<TEntity> source)
        {
            return (IGraphQLQueryable<TEntity>)
                GraphQLQueryableMethods.GetConfigureMethod(typeof(TEntity))
                    .Invoke(source, new object?[] { QueryableConfigurationKeys.DisabledIgnoreAttributes, true })!;
        }
    }
}
