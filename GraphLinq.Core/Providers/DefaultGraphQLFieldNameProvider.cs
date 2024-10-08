﻿using GraphLinq.Attributes;
using GraphLinq.Core.Providers.Abstractions;
using System.Reflection;

namespace GraphLinq.Core.Providers
{
    internal class DefaultGraphQLFieldNameProvider : IGraphQLFieldNameProvider
    {
        public string GetFieldName(MemberInfo member)
        {
            var propertyNameAttribute = member.GetCustomAttribute<GraphQLPropertyNameAttribute>(inherit: true);
            if (propertyNameAttribute is null) return FormatFieldName(member.Name);

            return propertyNameAttribute.Name;
        }

        private static string FormatFieldName(string name)
            => char.ToLower(name[0]) + name[1..];
    }
}
