using Autofac;

namespace Wallr.UI
{
    public class WallrUiModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<WallrUiServer>().As<IWallrUiServer>();
        }
    }
}