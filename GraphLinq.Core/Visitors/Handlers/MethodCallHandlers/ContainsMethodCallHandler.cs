using System.Collections;
using System.Linq.Expressions;
using System.Reflection;
using GraphLinq.Core.Models.Constants;
using GraphLinq.Core.Models.Internal;
using GraphLinq.Core.Providers.Abstractions;
using GraphLinq.Core.Utils;
using GraphLinq.Core.Visitors.Handlers.Abstractions;
using GraphLinq.Core.Visitors.Handlers.Models;

namespace GraphLinq.Core.Visitors.Handlers.MethodCallHandlers;

internal sealed class ContainsMethodCallHandler : MethodCallHandlerBase
{
    private static readonly MethodInfo NonGenericEnumerableContainsMethod =
        typeof(Enumerable)
            .GetMethods(BindingFlags.Static | BindingFlags.Public)
            .First(x => x.Name == "Contains" && x.GetParameters().Length == 2);

    private readonly IGraphQLValueFormatProvider _valueFormatProvider;

    public ContainsMethodCallHandler(
        IGraphQLValueFormatProvider valueFormatProvider)
    {
        _valueFormatProvider = valueFormatProvider;
    }

    public override MethodCallHandlerResult TryHandle(MethodCallExpression expression)
    {
        var failedResult = MethodCallHandlerResult.Failed();

        MemberExpression memberExpression = default!;
        ConstantExpression? constant = default!;

        if (expression.Arguments.Count == 1)
        {
            if (!ExpressionHelper.TryExtractMemberExpression(expression.Arguments[0], out memberExpression)) return failedResult;
            if (expression.Object is null || !ExpressionHelper.TryExtractConstantExpression(expression.Object, out constant)) return failedResult;
        }
        else if (expression.Arguments.Count == 2)
        {
            if (!ExpressionHelper.TryExtractMemberExpression(expression.Arguments[1], out memberExpression)) return failedResult;
            if (!ExpressionHelper.TryExtractConstantExpression(expression.Arguments[0], out constant)) return failedResult;
        }
        else
        {
            return failedResult;
        }

        var elementType = memberExpression.Type;

        var enumerableContainsMethod = NonGenericEnumerableContainsMethod.MakeGenericMethod(elementType);
        var hashSetContainsMethod = typeof(HashSet<>)
            .MakeGenericType(elementType)
            .GetMethods(BindingFlags.Instance | BindingFlags.Public)
            .First(x => x.Name == "Contains" && x.GetParameters().Length == 1);
        var listContainsMethod = typeof(List<>)
            .MakeGenericType(elementType)
            .GetMethods(BindingFlags.Instance | BindingFlags.Public)
            .First(x => x.Name == "Contains" && x.GetParameters().Length == 1);

        if (expression.Method != enumerableContainsMethod &&
            expression.Method != hashSetContainsMethod &&
            expression.Method != listContainsMethod)
        {
            return failedResult;
        }

        var value = constant.Value is IEnumerable array
            ? new WrappedEnumerable(array, _valueFormatProvider)
            : null;
        
        return MethodCallHandlerResult.Success(
            memberExpression,
            value,
            GraphQLOperations.In,
            GraphQLOperations.NotIn);
    }
}