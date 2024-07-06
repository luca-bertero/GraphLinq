namespace GraphLinq.Core.GraphQLQueryable.Abstractions
{
    public interface IOrderableGraphQLQueryable<out TEntity, out TProperty>
        : IGraphQLQueryable<TEntity>
    {
    }
}
