using System;
using Autofac;
using Wallr.Core;
using Wallr.Platform;

namespace Wallr
{
    public static class WallrLauncher
    {
        public static IDisposable LaunchWallr(IPlatform platform)
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule(new WallrModule(platform));
            IContainer container = containerBuilder.Build();

            container.Resolve<IWallrApplication>().Setup();
            
            return new WallrApplicationEnvironment(container);
        }
    }
}
