using System.Collections.Generic;
using Autofac;
using Wallr.ImageSource.Persistence;

namespace Wallr.ImageSource
{
    public class ImageSourceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ImageSourceConfigurations>().As<IImageSourceConfigurations>().As<IEnumerable<IImageSource>>().SingleInstance();
            builder.RegisterType<ImageSourceConverter>().As<IImageSourceConverter>();
            builder.RegisterType<ImageSourceConfigurationFactory>().As<IImageSourceConfigurationFactory>();
        }
    }
}