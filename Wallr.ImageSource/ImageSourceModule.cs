using Autofac;

namespace Wallr.ImageSource
{
    public class ImageSourceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ImageSources>().As<IImageSources>();
        }
    }
}