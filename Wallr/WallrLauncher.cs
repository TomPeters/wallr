using System;
using System.Threading.Tasks;
using Autofac;
using Wallr.Platform;
using Wallr.UI;
using Wallr.UI.SignalR;

namespace Wallr
{
    public static class WallrLauncher
    {
        public static async Task<IDisposable> LaunchWallr(IPlatform platform)
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule(new WallrModule(platform));
            IContainer container = containerBuilder.Build();

            await container.Resolve<IWallrApplication>().Setup();
            container.Resolve<ISignalRServer>().StartServer(); // TODO: Merge signalR and web server into composite server
            container.Resolve<IWallrUiServer>().StartServer();

            return new WallrApplicationEnvironment(container);
        }
    }
}
