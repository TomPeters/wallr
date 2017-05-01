using System;
using Microsoft.Owin.Host.HttpListener;
using Microsoft.Owin.Hosting;
using Microsoft.Owin.Security;
using Serilog;

namespace Wallr.UI.SignalR
{
    public interface ISignalRServer
    {
        void StartServer();
    }

    public class SignalRServer : ISignalRServer, IDisposable
    {
        // This ensures there is a reference to Owin.HttpListener & Owin.Security such that it will get copied on deployment 
        // They are required because they are used reflectively
#pragma warning disable 169
        private OwinHttpListener _httpListener;
        private ICertificateValidator _certificateValidator;
#pragma warning restore 169

        private readonly ILogger _logger;
        private readonly IClientEventSender _clientEventSender;
        private IDisposable _webApp;

        public SignalRServer(ILogger logger, IClientEventSender clientEventSender)
        {
            _logger = logger;
            _clientEventSender = clientEventSender;
        }

        public void StartServer()
        {
            _logger.Information("Starting SignalR server...");
            var url = "http://localhost:29485";
            _webApp = WebApp.Start<SignalRStartup>(url);
            _clientEventSender.StartSendingEvents();
            _logger.Information("SignalR server started at {Url}", url);
        }

        public void Dispose()
        {
            _webApp?.Dispose();
        }
    }
}