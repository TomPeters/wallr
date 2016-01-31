using Autofac;
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
        }
    }

    public class WallrNancyModule : Module
    {
        
    }
}