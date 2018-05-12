using System.Collections.Generic;

namespace app1
{
    public class Configuration
    {
        public Configuration()
        {
        }

        public ReRoute[] ReRoutes { get; set; }

        public GlobalConfiguration GlobalConfiguration { get; set; }

    }

    public class GlobalConfiguration{

    }

    public class ReRoute{
        public string DownstreamPathTemplate { get; set; }
        public string DownstreamScheme { get; set; }
        public string DownstreamHost { get; set; }
        public int DownstreamPort { get; set; }
        public string UpstreamPathTemplate { get; set; }
        public string[] UpstreamHttpMethod { get; set; }
        
    }
    /*
    public class ReRoute
    {
        public ReRoute()
          {
            UpstreamHttpMethod = new List<string>();
            AddHeadersToRequest = new Dictionary<string, string>();
            AddClaimsToRequest = new Dictionary<string, string>();
            RouteClaimsRequirement = new Dictionary<string, string>();
            AddQueriesToRequest = new Dictionary<string, string>();
            DownstreamHeaderTransform = new Dictionary<string, string>();
            FileCacheOptions = new FileCacheOptions();
            QoSOptions = new FileQoSOptions();
            RateLimitOptions = new FileRateLimitRule();
            AuthenticationOptions = new FileAuthenticationOptions();
            HttpHandlerOptions = new FileHttpHandlerOptions();
            UpstreamHeaderTransform = new Dictionary<string, string>();
            DownstreamHostAndPorts = new List<FileHostAndPort>();
        }

        public string DownstreamPathTemplate { get; set; }
        public string UpstreamPathTemplate { get; set; }
        public List<string> UpstreamHttpMethod { get; set; }
        public Dictionary<string, string> AddHeadersToRequest { get; set; }
        public Dictionary<string, string> UpstreamHeaderTransform { get; set; }
        public Dictionary<string, string> DownstreamHeaderTransform { get; set; }
        public Dictionary<string, string> AddClaimsToRequest { get; set; }
        public Dictionary<string, string> RouteClaimsRequirement { get; set; }
        public Dictionary<string, string> AddQueriesToRequest { get; set; }
        public string RequestIdKey { get; set; }
        //public FileCacheOptions FileCacheOptions { get; set; }
        public bool ReRouteIsCaseSensitive { get; set; }
        public string ServiceName { get; set; }
        public string DownstreamScheme {get;set;}
        public FileQoSOptions QoSOptions { get; set; }
        public string LoadBalancer { get;set; }
        public FileRateLimitRule RateLimitOptions { get; set; }
        public FileAuthenticationOptions AuthenticationOptions { get; set; }
        public FileHttpHandlerOptions HttpHandlerOptions { get; set; }
        public bool UseServiceDiscovery { get;set; }
        public List<FileHostAndPort> DownstreamHostAndPorts {get;set;}
        public string UpstreamHost { get; set; }
        public string Key { get;set; }
    }
     */
}
