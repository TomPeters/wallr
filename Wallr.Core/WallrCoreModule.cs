﻿using Autofac;

namespace Wallr.Core
{
    public class WallrCoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<WallrApplication>().As<IWallrApplication>();
            // nocommit, reimplement
//            builder.RegisterModule<ImageQueueModule>();
//            builder.RegisterModule<WallpaperModule>();
//            builder.RegisterModule<SourcesModule>();
        }
    }

//    public class SourcesModule : Module
//    {
//        protected override void Load(ContainerBuilder builder)
//        {
//            builder.RegisterType<ImageSourceProvider>().As<IImageSourceProvider>();
//            builder.RegisterType<SourceSerializer>().As<ISourceSerializer>();
//            builder.RegisterType<SourcesRepository>().As<ISourcesRepository>().As<ISourceConfigurationsProvider>();
//        }
//    }
//
//    public class ImageQueueModule : Module
//    {
//        protected override void Load(ContainerBuilder builder)
//        {
//            builder.RegisterType<ImageQueue>().As<IImageQueue>().SingleInstance();
//            builder.RegisterType<ImageQueuePopulator>().As<IImageQueuePopulator>();
//            builder.RegisterType<ImageQueueUpdateEvents>().As<IImageQueueUpdateEvents>();
//            builder.RegisterType<ImageQueueCoordinator>().As<IImageQueueCoordinator>();
//            builder.Register(c => new SubredditStatefulImageSource("wallpapers", c.Resolve<ILogger>())).As<IStatefulImageSource>(); // TODO: Should not register sources here
//        }
//    }
//
//    public class WallpaperModule : Module
//    {
//        protected override void Load(ContainerBuilder builder)
//        {
//            builder.RegisterType<WallpaperSetter>().As<IWallpaperSetter>();
//            builder.RegisterType<WallpaperUpdateEvents>().As<IWallpaperUpdateEvents>();
//            builder.RegisterType<WallpaperCoordinator>().As<IWallpaperCoordinator>();
//        }
//    }
}