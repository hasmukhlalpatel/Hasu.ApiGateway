using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace ApiGateway
{
    public static class MiddlewareExtensions{
        public static IApplicationBuilder UseReRouteMiddlewareMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ReRouteMiddlewareMiddleware>();
        }
        public static IServiceCollection AddMyMiddlewareMiddleware(this IServiceCollection services)
        {
            services.AddTransient<GatewayExceptionMiddleware>();

            services.AddTransient<ReRouteMiddlewareMiddleware>();
            return services;
        }

        public static IApplicationBuilder BuildApiGatewayPipeline(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<GatewayExceptionMiddleware>();
            builder.UseMiddleware<ReRouteMiddlewareMiddleware>();
            return builder;
        }
    }
}