using System;
using Autofac;
using FakeItEasy;
using Shouldly;
using Wallr.Platform;
using Wallr.UI;
using Wallr.UI.SignalR;
using Xunit;

namespace Wallr.Tests
{
    public class DependencyInjectionTests
    {
        private readonly ContainerBuilder _containerBuilder;

        public DependencyInjectionTests()
        {
            _containerBuilder = new ContainerBuilder();
            var platform = A.Fake<IPlatform>();
            _containerBuilder.RegisterModule(new WallrModule(platform));
        }

        [Fact]
        public void ResolveAndDisposeUIServers_CanBeResolvedAndDisposed()
        {
            using (IContainer container = _containerBuilder.Build())
            {
                container.Resolve<ISignalRServer>();
                container.Resolve<IWallrUiServer>();
            }
        }

        [Fact]
        public void ResolveAndDisposeWallrApplication_CanBeResolvedAndDisposed()
        {
            using (IContainer container = _containerBuilder.Build())
            {
                container.Resolve<IWallrApplication>();
            }
        }
    }
}