using Autofac;
using Serilog;
using Wallr.Core.Source;
using Wallr.ImageSource;
using Wallr.ImageSource.Subreddit;

namespace Wallr.Core
{
    public class WallrCoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<WallrApplication>().As<IWallrApplication>();
            builder.RegisterModule<ImageQueueModule>();
            builder.RegisterModule<WallpaperModule>();
            builder.RegisterModule<ImageSourceModule>();
        }
    }

    public class ImageSourceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SourcesRepository>().As<ISourcesRepository>();
            builder.RegisterType<ObservableSourcesRepository>().As<IImageSourceConfigurationProvider>();
            builder.Register(c => new SubredditImageSource("wallpapers", c.Resolve<ILogger>())).As<IImageSource>(); // TODO: don't hardcode sources
        }
    }

    public class ImageQueueModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ImageQueue>().As<IImageQueue>().SingleInstance();
            builder.RegisterType<ImageQueuePopulator>().As<IImageQueuePopulator>();
            builder.RegisterType<ImageQueueUpdateEvents>().As<IImageQueueUpdateEvents>();
            builder.RegisterType<ImageQueueCoordinator>().As<IImageQueueCoordinator>();
        }
    }

    public class WallpaperModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<WallpaperSetter>().As<IWallpaperSetter>();
            builder.RegisterType<WallpaperUpdateEvents>().As<IWallpaperUpdateEvents>();
            builder.RegisterType<WallpaperCoordinator>().As<IWallpaperCoordinator>();
        }
    }
}