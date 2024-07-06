namespace GraphLinq.Core.Providers.Abstractions
{
    public interface IGraphQLOffsetPaginationStringProvider
    {
        string? BuildSkip(int? skip);
        string? BuildTake(int? take);
    }
}
