namespace GraphLinq.Core.Providers.Abstractions
{
    public interface IGraphQLEndpointProvider
    {
        string GetGraphQLEndpoint(Type entityType);
    }
}
