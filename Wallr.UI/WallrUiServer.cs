using System;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Hosting.Self;
using Serilog;

namespace Wallr.UI
{
    public interface IWallrUiServer
    {
        void StartServer();
    }

    public class WallrUiServer : IWallrUiServer, IDisposable // TODO: Implement server
    {
        private readonly INancyBootstrapper _nancyBootstrapper;
        private readonly ILogger _logger;
        private NancyHost _host;

        public WallrUiServer(INancyBootstrapper nancyBootstrapper, ILogger logger)
        {
            _nancyBootstrapper = nancyBootstrapper;
            _logger = logger;
        }

        public void Dispose()
        {
            _host?.Dispose();
        }

        public void StartServer()
        {
            var hostConfiguration = new HostConfiguration();
            hostConfiguration.RewriteLocalhost = false;
            _host = new NancyHost(_nancyBootstrapper, hostConfiguration, new Uri("http://localhost:36844/"));
            _logger.Information("Web server starting...");
            _host.Start();
            _logger.Information("Web server started");
        }
    }
}