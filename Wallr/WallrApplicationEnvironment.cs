using System;
using Autofac;
using Serilog;

namespace Wallr
{
    public class WallrApplicationEnvironment : IDisposable
    {
        private readonly IContainer _container;

        public WallrApplicationEnvironment(IContainer container)
        {
            _container = container;
        }

        public void Dispose()
        {
            _container.Resolve<ILogger>().Information("Application Exiting");
            _container.Dispose();
        }
    }
}