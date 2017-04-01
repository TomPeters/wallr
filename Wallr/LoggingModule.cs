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
            builder.Register(c => CreateLogger(c.Resolve<IConfiguration>())).As<ILogger>().SingleInstance();
        }

        private ILogger CreateLogger(IConfiguration configuration)
        {
            LoggerConfiguration loggerConfiguration = new LoggerConfiguration()
                .Enrich.WithProperty("Product", "Wallr")
                .Enrich.WithProperty("InstanceId", Guid.NewGuid())
                .MinimumLevel.Debug();
            return configuration.LoggerSinks
                .Aggregate(loggerConfiguration, (current, sinkConfiguration) => sinkConfiguration(current.WriteTo))
                .CreateLogger();
        }
    }
}