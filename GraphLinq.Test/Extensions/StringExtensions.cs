using System;

namespace GraphLinq.Tests.TestsInfrastructure.Extensions
{
    internal static class StringExtensions
    {
        public static string[] Tokenize(this string query)
        {
            return query.Trim().Split(new[] { ' ', '\n', '\t', '\r' }, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
