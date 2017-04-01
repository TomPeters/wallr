using Autofac;
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
            builder.RegisterInstance(_platform).As<IPlatform>(); // nocommit, don't register IPlatform, register the segregated interface instead
            builder.RegisterModule<LoggingModule>();
            builder.RegisterModule<WallrCoreModule>();
            builder.RegisterModule<WallrUiModule>();
            builder.RegisterType<OpenConfigPageOption>().As<IQuickUseOption>();
        }
    }
}