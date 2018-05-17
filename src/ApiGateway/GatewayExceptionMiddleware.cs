using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ApiGateway
{
    public class GatewayExceptionMiddleware : GatewayMiddlewareBase
    {
        public override async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception exception)
            {
                context.Response.StatusCode = 500;

                await context.Response.WriteAsync("An error has occured");
            }
        }
    }
}