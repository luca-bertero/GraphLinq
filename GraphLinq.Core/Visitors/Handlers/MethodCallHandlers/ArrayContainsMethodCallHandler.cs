using System.Linq.Expressions;
using System.Reflection;
using GraphLinq.Core.Extensions;
using GraphLinq.Core.Models.Constants;
using GraphLinq.Core.Utils;
using GraphLinq.Core.Visitors.Handlers.Abstractions;
using GraphLinq.Core.Visitors.Handlers.Models;

namespace GraphLinq.Core.Visitors.Handlers.MethodCallHandlers;

internal sealed class ArrayContainsMethodCallHandler : MethodCallHandlerBase
{
    private static readonly MethodInfo NonGenericEnumerableContainsMethod =
        typeof(Enumerable)
            .GetMethods(BindingFlags.Static | BindingFlags.Public)
            .First(x => x.Name == "Contains" && x.GetParameters().Length == 2);
    
    public override MethodCallHandlerResult TryHandle(MethodCallExpression expression)
    {
        var failedResult = MethodCallHandlerResult.Failed();

        if (expression.Arguments.Count != 2) return failedResult;
        if (!ExpressionHelper.TryExtractMemberExpression(expression.Arguments[0], out var memberExpression)) return failedResult;
        if (!ExpressionHelper.TryExtractConstantExpression(expression.Arguments[1], out var constantExpression)) return failedResult;

        if (!memberExpression.Type.TryGetCollectionElementType(out var elementType)) return failedResult;

        var containsMethod = NonGenericEnumerableContainsMethod.MakeGenericMethod(elementType);
        if (expression.Method != containsMethod) return failedResult;

        return MethodCallHandlerResult.Success(
            memberExpression, 
            constantExpression.Value, 
            GraphQLOperations.Contains,
            GraphQLOperations.NotContains);
    }
}