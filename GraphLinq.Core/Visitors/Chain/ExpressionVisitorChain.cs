using GraphLinq.Core.Providers;
using GraphLinq.Core.Visitors.Handlers.MethodCallHandlers;
namespace GraphLinq.Core.Visitors.Chain
{
    internal class ExpressionVisitorChain : BaseExpressionVisitorChain
    {
        public ExpressionVisitorChain()
        {
            Register(new AllMethodCallHandler());
            
            Register(new AnyMethodCallHandler());
            
            Register(new AnyMethodCallHandlerEmpty());

            Register(new ArrayContainsMethodCallHandler());
            
            Register(new ContainsMethodCallHandler(
                new DefaultGraphQLValueFormatProvider()));
            
            Register(new StringContainsMethodCallHandler());
            
            Register(new StringEndsWithMethodCallHandler());
            
            Register(new StringStartsWithMethodCallHandler());
        }
    }
}
