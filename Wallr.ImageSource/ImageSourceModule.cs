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
            builder.RegisterType<ImageSourceConfigurations>().As<IImageSourceConfigurations>().As<IEnumerable<IImageSourceConfiguration>>().SingleInstance();
            builder.RegisterType<ImageSources>().As<IImageSources>();
            builder.RegisterType<ImageSourceFactory>().As<IImageSourceFactory>();
            builder.RegisterType<ImageSourcePluginFactory>().As<IImageSourcePluginFactory>();
            builder.RegisterType<ImageSourceConverter>().As<IImageSourceConverter>();
            builder.RegisterType<ImageSourceConfigurationFactory>().As<IImageSourceConfigurationFactory>();
            builder.RegisterType<TypedSettingsConverter>().As<ITypedSettingsConverter>();

            #region temporary (for testing)
            builder.RegisterType<SubredditImageSource>(); // nocommit, don't have a dependency on specific sources, should use reflection to load them
            builder.RegisterType<TestingSources>();
            #endregion
        }
    }
}