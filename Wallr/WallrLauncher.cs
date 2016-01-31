using System;
using Autofac;
using Wallr.Core;
using Wallr.Platform;
using Wallr.UI;
using Wallr.UI.SignalR;

namespace Wallr
{
    public static class WallrLauncher
    {
        public static IDisposable LaunchWallr(IPlatform platform)
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule(new WallrModule(platform));
            IContainer container = containerBuilder.Build();

            container.Resolve<ISignalRServer>().StartServer(); // TODO: Merge signalR and web server into composite server
            container.Resolve<IWallrUiServer>().StartServer();
            container.Resolve<IWallrApplication>().Setup();


            return new WallrApplicationEnvironment(container);
        }
    }
}
