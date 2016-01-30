using System;
using Autofac;
using Wallr.Core;
using Wallr.Platform;
using Wallr.UI;

namespace Wallr
{
    public static class WallrLauncher
    {
        public static IDisposable LaunchWallr(IPlatform platform)
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule(new WallrModule(platform));
            IContainer container = containerBuilder.Build();

            container.Resolve<IWallrUiServer>().StartServer();
            container.Resolve<IWallrApplication>().Setup();

            return new WallrApplicationEnvironment(container);
        }
    }
}
