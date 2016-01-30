using System;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Hosting.Self;

namespace Wallr.UI
{
    public interface IWallrUiServer
    {
        void StartServer();
    }

    public class WallrUiServer : IWallrUiServer, IDisposable // TODO: Implement server
    {
        private readonly INancyBootstrapper _nancyBootstrapper;
        private NancyHost _host;

        public WallrUiServer(INancyBootstrapper nancyBootstrapper)
        {
            _nancyBootstrapper = nancyBootstrapper;
        }

        public void Dispose()
        {
            _host?.Dispose();
        }

        public void StartServer()
        {
            var hostConfiguration = new HostConfiguration();
            hostConfiguration.RewriteLocalhost = false;
            _host = new NancyHost(_nancyBootstrapper, hostConfiguration, new Uri("http://localhost:36849"));
            _host.Start();
        }
    }
}