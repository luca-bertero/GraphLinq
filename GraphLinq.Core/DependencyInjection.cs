using GraphLinq.Core.Providers;
using GraphLinq.Core.Providers.Abstractions;
using GraphLinq.Core.Visitors.Handlers.Abstractions;
using GraphLinq.Core.Visitors.Handlers.MethodCallHandlers;

namespace SmartGraphQLClient.Core
{
    public static class DependencyInjection
    {
        /// <summary>
        /// Add <see cref="GraphQLHttpClient"/> and
        /// application services required for working <see cref="GraphQLHttpClient"/>
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddSmartGraphQLClient(this IServiceCollection services)
        {
            
            // Providers
            services.TryAddTransient<IGraphQLBodyStringProvider, DefaultGraphQLBodyStringProvider>();
            services.TryAddTransient<IGraphQLWhereStringProvider, DefaultGraphQLWhereStringProvider>();
            services.TryAddTransient<IGraphQLOrderStringProvider, DefaultGraphQLOrderStringProvider>();
            services.TryAddTransient<IGraphQLOffsetPaginationStringProvider, DefaultGraphQLOffsetPaginationStringProvider>();
            services.TryAddTransient<IGraphQLFieldNameProvider, DefaultGraphQLFieldNameProvider>();
            services.TryAddTransient<IGraphQLValueFormatProvider, DefaultGraphQLValueFormatProvider>();

            // MethodCallHandlers
            services.AddTransient<IMethodCallHandler, AllMethodCallHandler>();
            services.AddTransient<IMethodCallHandler, AnyMethodCallHandler>();
            services.AddTransient<IMethodCallHandler, AnyMethodCallHandlerEmpty>();
            services.AddTransient<IMethodCallHandler, ArrayContainsMethodCallHandler>();
            services.AddTransient<IMethodCallHandler, ContainsMethodCallHandler>();
            services.AddTransient<IMethodCallHandler, StringContainsMethodCallHandler>();
            services.AddTransient<IMethodCallHandler, StringEndsWithMethodCallHandler>();
            services.AddTransient<IMethodCallHandler, StringStartsWithMethodCallHandler>();
            
            return services;
        }
    }
}
