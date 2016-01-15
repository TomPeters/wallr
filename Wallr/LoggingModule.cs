using System;
using System.Linq;
using Autofac;
using Serilog;
using Wallr.Platform;

namespace Wallr
{
    public class LoggingModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => CreateLogger(c.Resolve<IPlatform>())).As<ILogger>().SingleInstance();
        }

        private ILogger CreateLogger(IPlatform platform)
        {
            LoggerConfiguration loggerConfiguration = new LoggerConfiguration()
                .Enrich.WithProperty("Product", "Wallr")
                .Enrich.WithProperty("InstanceId", Guid.NewGuid())
                .MinimumLevel.Debug();
            return platform.LoggerSinks
                .Aggregate(loggerConfiguration, (current, sinkConfiguration) => sinkConfiguration(current.WriteTo))
                .CreateLogger();
        }
    }
}