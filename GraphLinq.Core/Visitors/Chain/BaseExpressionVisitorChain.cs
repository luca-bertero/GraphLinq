using GraphLinq.Core.Visitors.Handlers.Abstractions;
using GraphLinq.Core.Visitors.Handlers.Models;
using System.Linq.Expressions;

namespace GraphLinq.Core.Visitors.Chain
{
    internal abstract class BaseExpressionVisitorChain
    {
        protected readonly IList<IMethodCallHandler> _handlers = [];

        public void Register(IMethodCallHandler handler)
        {
            _handlers.Add(handler);
        }

        public MethodCallHandlerResult ExecuteAsync(MethodCallExpression expression)
        {
            MethodCallHandlerResult result = default!;
            foreach (var handler in _handlers)
            {
                handler.TryHandle(expression);

                if (result.CanHandle)
                {
                    return result;
                }
            }

            return result ?? MethodCallHandlerResult.Failed();
        }
    }
}
