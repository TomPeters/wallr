using Autofac;
using Wallr.Core;
using Wallr.Platform;
using Wallr.UI;

namespace Wallr
{
    public class WallrModule : Module
    {
        private readonly IPlatform _platform;

        public WallrModule(IPlatform platform)
        {
            _platform = platform;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(_platform).As<IPlatform>();
            builder.RegisterModule<LoggingModule>();
            builder.RegisterModule<WallrCoreModule>();
            builder.RegisterModule<WallrUiModule>();
        }
    }
}