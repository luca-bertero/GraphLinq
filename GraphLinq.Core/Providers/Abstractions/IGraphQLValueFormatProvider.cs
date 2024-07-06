namespace GraphLinq.Core.Providers.Abstractions
{
    public interface IGraphQLValueFormatProvider
    {
        string GetFormattedValue(object? value);
    }
}
