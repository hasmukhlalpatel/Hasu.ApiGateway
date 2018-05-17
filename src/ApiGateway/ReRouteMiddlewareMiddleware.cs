using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace ApiGateway
{
    public class ReRouteMiddlewareMiddleware : GatewayMiddlewareBase
    {
        private static string [] reseveredHeaders = new []{"Date","Transfer-Encoding", "Server"};
        public override async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var httpMeessgeHandle = new HttpClientHandler(){
                AllowAutoRedirect = true,
                UseCookies = true,
                CookieContainer = new System.Net.CookieContainer()
            };


            httpMeessgeHandle.ServerCertificateCustomValidationCallback = (req,cert, chain, errs )=> true;

            var httpClinet = new HttpClient(httpMeessgeHandle);
            httpClinet.BaseAddress= new Uri( "http://localhost:54062/");
            
            var request = context.Request;

            var requestUrl = request.Path != "/" 
                ? $"{request.Path}/{request.QueryString}" 
                : $"/{request.QueryString}";

            var requestMethod = new HttpMethod(request.Method);
            var requestMessage = new HttpRequestMessage(requestMethod, requestUrl);
            
            if(request.ContentLength>0){
                requestMessage.Content = new StreamContent(request.Body);
            }

            foreach (var header in request.Headers)
            {
                requestMessage.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray());
            }

            var cookies ="";
            foreach (var cooky in request.Cookies)
            {
                cookies += $"{cooky.Key}={cooky.Value};";
            }
            
            requestMessage.Headers.TryAddWithoutValidation("Cookie",cookies);

            var responseMessage = await  httpClinet.SendAsync(requestMessage);

            foreach (var header in responseMessage.Headers)
            {
                if(!reseveredHeaders.Contains(header.Key)){
                context.Response.Headers[header.Key] = new Microsoft.Extensions.Primitives.StringValues(header.Value.ToArray());
                }
            }
            
            //context.Response.Cookies.Append("HPTestCooky","Test Val");
           // context.Response.Headers["HPTEst"] = responseMessage.Headers.First(x=>x.Key== "HPTEst").Value.First();
           context.Response.StatusCode = (int)responseMessage.StatusCode;
           if(responseMessage.Content.Headers.ContentType != null)
           {
               context.Response.ContentType = responseMessage.Content.Headers.ContentType.ToString();
           }

            var responseStr= await responseMessage.Content.ReadAsStringAsync();
            //var responseStream= await responseMessage.Content.ReadAsStreamAsync();

            await context.Response.WriteAsync(responseStr);
           
        }
    }
}
