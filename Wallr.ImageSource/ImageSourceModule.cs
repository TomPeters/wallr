using System.Collections.Generic;
using Autofac;
using Wallr.ImageSource.Persistence;
using Wallr.ImageSource.Subreddit;

namespace Wallr.ImageSource
{
    public class ImageSourceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ImageSourceConfigurations>().As<IImageSourceConfigurations>().As<IEnumerable<IImageSource>>().SingleInstance();
            builder.RegisterType<ImageSources>().As<IImageSources>().As<IEnumerable<IImageSource>>();
            builder.RegisterType<ImageSourceFactory>().As<IImageSourceFactory>();
            builder.RegisterType<ImageSourcePluginFactory>().As<IImageSourcePluginFactory>();
            builder.RegisterType<ImageSourceConverter>().As<IImageSourceConverter>();
            builder.RegisterType<ImageSourceConfigurationFactory>().As<IImageSourceConfigurationFactory>();

            #region temporary (for testing)
            builder.RegisterType<SubredditImageSource>(); // nocommit, don't have a dependency on specific sources, should use reflection to load them
            builder.RegisterType<TestingSources>();
            #endregion
        }
    }
}