using Autofac;

namespace Wallr.UI.Middleware
{
    public class MiddlewareModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<LoggingMiddleware>()
                .As<IBeforeRequestMiddleware>()
                .As<IAfterRequestMiddleware>()
                .As<IOnErrorMiddleware>();
        }
    }
}