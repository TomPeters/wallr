using System;
using Autofac;
using Wallr.ImageQueue.Persistence;
using Wallr.ImageSource;

namespace Wallr.ImageQueue
{
    public class ImageQueueModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ImageQueue>().Named<IImageQueue>("inner").SingleInstance();

            builder.RegisterType<PersistingImageQueue>();
            builder.RegisterDecorator<IImageQueue>((context, queue) => context.Resolve<PersistingImageQueue>(new TypedParameter(typeof(IImageQueue), queue)), "inner")
                .Named<IPersistingImageQueue>("inner");

            builder.RegisterType<ObservableImageQueue>();
            builder.RegisterDecorator<IPersistingImageQueue>((context, inner)
                    => context.Resolve<ObservableImageQueue>(new TypedParameter(typeof(IPersistingImageQueue), inner)), "inner")
                .As<IObservableImageQueue>().As<IPersistingImageQueue>().As<IImageQueue>().As<IImageQueueEvents>().SingleInstance();

            builder.RegisterType<ImageQueueSerializer>().As<IImageQueueSerializer>();
            builder.RegisterType<SourceQualifiedImageIdConverter>().As<ISourceQualifiedImageIdConverter>();
        }
    }
}