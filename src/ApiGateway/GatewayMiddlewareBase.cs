using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ApiGateway
{
    public abstract class GatewayMiddlewareBase : IMiddleware
    {
        private const string ConstGatewayContext = "_GatewayContext_";

        protected GatewayContext GetGatewayContext(HttpContext context)
        {
            if (context.Items.ContainsKey(ConstGatewayContext))
            {
                return (GatewayContext)context.Items[ConstGatewayContext];
            }

            var gatewayContext = new GatewayContext();

            context.Items[ConstGatewayContext] = gatewayContext;

            return gatewayContext;
        }

        public abstract Task InvokeAsync(HttpContext context, RequestDelegate next);

    }

    public class GatewayContext
    {

    }
}