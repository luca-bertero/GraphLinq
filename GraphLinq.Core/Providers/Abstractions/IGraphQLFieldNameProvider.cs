using System.Reflection;

namespace GraphLinq.Core.Providers.Abstractions
{
    public interface IGraphQLFieldNameProvider
    {
        string GetFieldName(MemberInfo member);
    }
}
