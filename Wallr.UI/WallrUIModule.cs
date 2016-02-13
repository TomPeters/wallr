using Autofac;
using Microsoft.AspNet.SignalR;
using Nancy.Bootstrapper;
using Wallr.UI.SignalR;

namespace Wallr.UI
{
    public class WallrUiModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<WallrUiServer>().As<IWallrUiServer>();
            builder.RegisterType<WallrNancyBootstrapper>().As<INancyBootstrapper>();
            builder.RegisterType<SignalRServer>().As<ISignalRServer>();
            builder.Register(c => GlobalHost.ConnectionManager.GetHubContext<WallrHub>()).As<IHubContext>();
            builder.RegisterType<ClientEventSender>().As<IClientEventSender>();
        }
    }
}