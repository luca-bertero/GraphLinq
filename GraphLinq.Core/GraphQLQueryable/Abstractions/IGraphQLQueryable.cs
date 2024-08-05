namespace GraphLinq.Core.GraphQLQueryable.Abstractions
{
    public interface IGraphQLQueryable<out TEntity>
    {
        public string BuildQuery();
    }
}
