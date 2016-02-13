using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using Autofac;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Bootstrappers.Autofac;
using Nancy.Conventions;
using Serilog;
using Wallr.Core;
using Wallr.Platform;
using Wallr.UI.Middleware;

namespace Wallr.UI
{
    public class WallrNancyBootstrapper : AutofacNancyBootstrapper
    {
        private readonly ILifetimeScope _parentScope;

        public WallrNancyBootstrapper(ILifetimeScope parentScope)
        {
            _parentScope = parentScope;
        }

        protected override void ConfigureConventions(NancyConventions nancyConventions)
        {
            nancyConventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("/static", "content"));
            base.ConfigureConventions(nancyConventions);
        }

        protected override void ApplicationStartup(ILifetimeScope container, IPipelines pipelines)
        {
            container.Update(b => b.RegisterModule<MiddlewareModule>());
        }

        protected override void ConfigureRequestContainer(ILifetimeScope container, NancyContext context)
        {
            container.Update(b => 
                b.Register(c => 
                    _parentScope.Resolve<ILogger>()
                        .ForContext("RequestId", Guid.NewGuid())
                        .ForContext("Url", context.Request.Url)
                        .ForContext("Method", context.Request.Method))
                    .As<ILogger>()
            );
            container.Update(b => b.Register(c => _parentScope.Resolve<IPlatform>()).As<IPlatform>());
            container.Update(b => b.Register(c => _parentScope.Resolve<IImageQueue>()).As<IImageQueue>());
        }

        protected override void RequestStartup(ILifetimeScope container, IPipelines pipelines, NancyContext context)
        {
            pipelines.BeforeRequest = container.Resolve<IEnumerable<IBeforeRequestMiddleware>>()
                .Aggregate(pipelines.BeforeRequest, (pipeline, middleware) => pipeline + middleware.Invoke);

            pipelines.AfterRequest = container.Resolve<IEnumerable<IAfterRequestMiddleware>>()
                .Aggregate(pipelines.AfterRequest, (pipeline, middleware) => pipeline + middleware.Invoke);

            pipelines.OnError = container.Resolve<IEnumerable<IOnErrorMiddleware>>()
                .Aggregate(pipelines.OnError, (pipeline, middleware) => pipeline + middleware.Invoke);
        }
    }
}