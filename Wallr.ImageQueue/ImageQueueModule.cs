﻿using Autofac;
using Wallr.ImageQueue.Persistence;

namespace Wallr.ImageQueue
{
    public class ImageQueueModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ImageQueue>().As<IImageQueue>().SingleInstance();
            builder.RegisterType<SourceQualifiedImageIdConverter>().As<ISourceQualifiedImageIdConverter>();
        }
    }
}