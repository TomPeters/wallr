using Autofac;

namespace Wallr.WallpaperUpdates
{
    public class WallpaperUpdatesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<WallpaperUpdater>().As<IWallpaperUpdater>();
        }
    }
}