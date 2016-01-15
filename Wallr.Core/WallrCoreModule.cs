using Autofac;
using Serilog;
using Wallr.ImageSource;
using Wallr.ImageSource.Subreddit;

namespace Wallr.Core
{
    public class WallrCoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<WallrApplication>().As<IWallrApplication>();
            builder.RegisterModule<ImageStreamModule>();
            builder.RegisterModule<WallpaperModule>();
        }
    }

    public class ImageStreamModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ImageStream>().As<IImageStream>().SingleInstance();
            builder.RegisterType<ImageStreamPopulator>().As<IImageStreamPopulator>();
            builder.RegisterType<ImageStreamUpdateEvents>().As<IImageStreamUpdateEvents>();
            builder.RegisterType<ImageStreamCoordinator>().As<IImageStreamCoordinator>();
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