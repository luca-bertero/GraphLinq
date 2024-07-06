using GraphLinq.Attributes;
using GraphLinq.Core.Extensions;
using GraphLinq.Core.Providers.Abstractions;
using System.Reflection;

namespace GraphLinq.Core.Visitors.Abstractions
{
    internal abstract class VisitorBase
    {
        protected static string FormatFieldName(MemberInfo memberInfo)
            => GetFieldName(memberInfo);
        
        protected static string FormatValue(object? value)
            => GetFormattedValue(value);

        private static string GetFieldName(MemberInfo member)
        {
            var propertyNameAttribute = member.GetCustomAttribute<GraphQLPropertyNameAttribute>(inherit: true);
            if (propertyNameAttribute is null) return FormatFieldName(member.Name);

            return propertyNameAttribute.Name;
        }

        private static string FormatFieldName(string name)
            => char.ToLower(name[0]) + name[1..];

        private static string GetFormattedValue(object? value)
        {
            if (value is null) return "null";

            if (value is bool bValue) return bValue ? "true" : "false";

            if (value is string sValue)
            {
                return EscapeStringValue(sValue);
            }

            if (value is DateTime dtValue) return dtValue.ToUniversalIso8601();

            return value.ToString() ?? string.Empty;
        }

        private static string EscapeStringValue(string value)
        {
            value = value
                .Replace(@"\", @"\\")
                .Replace("\"", "\\\"")
                .Replace("\n", @"\n")
                .Replace("\r", @"\r")
                .Replace("\t", @"\t")
                .Replace("\0", @"\0")
                .Replace("\b", @"\b");

            return $"\"{value}\"";
        }
    }
}
