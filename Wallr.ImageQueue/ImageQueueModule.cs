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
            builder.RegisterType<ObservableImageQueue>();
            builder.RegisterDecorator<IImageQueue>((context, inner) => context.Resolve<ObservableImageQueue>(new TypedParameter(typeof(IImageQueue), inner)), "inner")
                .As<IObservableImageQueue>().As<IImageQueue>().As<IImageQueueEvents>().SingleInstance();
            builder.RegisterType<SourceQualifiedImageIdConverter>().As<ISourceQualifiedImageIdConverter>();
        }
    }
}