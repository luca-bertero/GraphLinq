using System.Linq.Expressions;
using GraphLinq.Core.Visitors.Handlers.Models;

namespace GraphLinq.Core.Visitors.Handlers.Abstractions;

public abstract class MethodCallHandlerBase : IMethodCallHandler
{
    public abstract MethodCallHandlerResult TryHandle(MethodCallExpression expression);
}