using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Owin;

[assembly: OwinStartup(typeof(com.AppliedLine.CargoCanal.WebAPI.Startup))]

namespace com.AppliedLine.CargoCanal.WebAPI
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888

            // Setup the CORS middleware to run before SignalR.
            // By default this will allow all origins. You can 
            // configure the set of origins and/or http verbs by
            // providing a cors options with a different policy.
            app.UseCors(CorsOptions.AllowAll);
            var hubConfig = new HubConfiguration
            {
                // You can enable JSONP by uncommenting line below.
                // JSONP requests are insecure but some older browsers (and some
                // versions of IE) require JSONP to work cross domain
                // EnableJSONP = true
            };

            app.MapSignalR(hubConfig);
        }
    }
}
