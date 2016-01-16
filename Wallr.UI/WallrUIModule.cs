using Autofac;
using Nancy.Bootstrapper;

namespace Wallr.UI
{
    public class WallrUiModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<WallrUiServer>().As<IWallrUiServer>();
            builder.RegisterType<WallrNancyBootstrapper>().As<INancyBootstrapper>();
        }
    }

    public class WallrNancyModule : Module
    {
        
    }
}