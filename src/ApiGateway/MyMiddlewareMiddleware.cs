using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace app1
{
    public class MyMiddlewareMiddleware : IMiddleware
    {
        private static string [] reseveredHeaders = new []{"Date","Transfer-Encoding", "Server"};
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
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

    public static class MiddlewareExtensions{
        public static IApplicationBuilder UseMyMiddlewareMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<MyMiddlewareMiddleware>();
        }
        public static IServiceCollection AddMyMiddlewareMiddleware(this IServiceCollection services)
        {
            return services.AddTransient<MyMiddlewareMiddleware>();
        }
    }

    public class RouteHttpClientFactory{

        public IRouteHttpClient Build(HttpContext context){
            var routeHttpClient = new RouteHttpClient("http://localhost:54062/");
            return routeHttpClient;
        }

    }
    
    public interface IRouteHttpClient :IDisposable
    {
         Task<HttpResponseMessage> SendAsync(HttpRequestMessage request);
    }

    public class RouteHttpClient: IRouteHttpClient
    {
        HttpClient _httpClient;
        public RouteHttpClient(string baseAddressUrl)
        {
            var httpMeessgeHandle = new HttpClientHandler(){
                AllowAutoRedirect = true,
                UseCookies = true,
                CookieContainer = new System.Net.CookieContainer()
            };
            
            httpMeessgeHandle.ServerCertificateCustomValidationCallback = (req,cert, chain, errs )=> true;

            _httpClient = new HttpClient(httpMeessgeHandle);
            _httpClient.BaseAddress= new Uri(baseAddressUrl);
        }

        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
                return await _httpClient.SendAsync(request);
        }
        bool disposed = false;

        public void Dispose()
        { 
            Dispose(true);
            GC.SuppressFinalize(this);           
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return; 

            if (disposing) {
                _httpClient.Dispose();
            }
            disposed = true;
        }

        ~RouteHttpClient()
        {
            Dispose(false);
        }
    }

}
