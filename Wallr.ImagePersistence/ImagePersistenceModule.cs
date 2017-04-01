using Autofac;

namespace Wallr.ImagePersistence
{
    public class ImagePersistenceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ImageRepository>().As<IImageRepository>();
            builder.RegisterType<ImageSaver>().As<IImageSaver>();
        }
    }
}