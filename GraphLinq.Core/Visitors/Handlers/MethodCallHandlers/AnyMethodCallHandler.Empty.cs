using System.Linq.Expressions;
using System.Reflection;
using GraphLinq.Core.Extensions;
using GraphLinq.Core.Models.Constants;
using GraphLinq.Core.Utils;
using GraphLinq.Core.Visitors.Handlers.Abstractions;
using GraphLinq.Core.Visitors.Handlers.Models;

namespace GraphLinq.Core.Visitors.Handlers.MethodCallHandlers;

internal sealed class AnyMethodCallHandlerEmpty : MethodCallHandlerBase
{
    private static readonly MethodInfo NonGenericEnumerableAnyMethod =
        typeof(Enumerable)
            .GetMethods(BindingFlags.Static | BindingFlags.Public)
            .First(x => x.Name == "Any" && x.GetParameters().Length == 1);

    public override MethodCallHandlerResult TryHandle(MethodCallExpression expression)
    {
        var failedResult = MethodCallHandlerResult.Failed();

        if (expression.Arguments.Count != 1) return failedResult;
        if (!ExpressionHelper.TryExtractMemberExpression(expression.Arguments[0], out var memberExpression)) return failedResult;

        if (!memberExpression.Type.TryGetCollectionElementType(out var elementType)) return failedResult;

        var anyMethod = NonGenericEnumerableAnyMethod.MakeGenericMethod(elementType);

        if (expression.Method != anyMethod) return failedResult;

        // any: true / any: false
        return MethodCallHandlerResult.Success(
            memberExpression,
            (bool inverted) => !inverted ? true : false,
            GraphQLOperations.Any,
            GraphQLOperations.Any);
    }
}