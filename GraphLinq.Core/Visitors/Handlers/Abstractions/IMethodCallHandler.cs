using System.Linq.Expressions;
using GraphLinq.Core.Visitors.Handlers.Models;

namespace GraphLinq.Core.Visitors.Handlers.Abstractions;

public interface IMethodCallHandler
{
    MethodCallHandlerResult TryHandle(MethodCallExpression expression);
}