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
            builder.RegisterType<ImageSourceConfigurationProvider>().As<IImageSourceConfigurationProvider>(); // nocommit new module for sources
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
            builder.Register(c => new SubredditImageSource("wallpapers", c.Resolve<ILogger>())).As<IImageSource>(); // TODO: Should not register sources here
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