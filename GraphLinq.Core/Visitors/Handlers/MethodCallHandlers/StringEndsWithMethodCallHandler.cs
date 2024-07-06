using System.Linq.Expressions;
using System.Reflection;
using GraphLinq.Core.Extensions;
using GraphLinq.Core.Models.Constants;
using GraphLinq.Core.Utils;
using GraphLinq.Core.Visitors.Handlers.Abstractions;
using GraphLinq.Core.Visitors.Handlers.Models;

namespace GraphLinq.Core.Visitors.Handlers.MethodCallHandlers;

internal sealed class StringEndsWithMethodCallHandler : MethodCallHandlerBase
{
    private static readonly MethodInfo StringEndsWithMethod = typeof(string)
        .GetMethods(BindingFlags.Instance | BindingFlags.Public)
        .First(x => x.Name == "EndsWith" && x.GetParameters().Length == 1);

    public override MethodCallHandlerResult TryHandle(MethodCallExpression expression)
    {
        var failedResult = MethodCallHandlerResult.Failed();

        if (expression.Arguments.Count != 1) return failedResult;
        if (expression.Object is null || !ExpressionHelper.TryExtractMemberExpression(expression.Object, out var memberExpression)) return failedResult;
        if (!ExpressionHelper.TryExtractConstantExpression(expression.Arguments[0], out var constantExpression)) return failedResult;
        if (expression.Method != StringEndsWithMethod) return failedResult;

        return MethodCallHandlerResult.Success(
            memberExpression, 
            constantExpression.Value, 
            GraphQLOperations.EndsWith,
            GraphQLOperations.NotEndsWith);
    }
}