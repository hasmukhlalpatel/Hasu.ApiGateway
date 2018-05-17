using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ApiGateway
{

    public class RouteHttpClientFactory
    {

        public IRouteHttpClient Build(HttpContext context)
        {
            var routeHttpClient = new RouteHttpClient("http://localhost:54062/");
            return routeHttpClient;
        }

    }

    public interface IRouteHttpClient : IDisposable
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