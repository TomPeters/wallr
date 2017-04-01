using Autofac;
using Wallr.ImagePersistence;
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
            builder.RegisterInstance(_platform)
                .As<IConfiguration>()
                .As<ISetup>()
                .As<INavigation>()
                .As<IWallpaperEnvironment>()
                .As<IPersistence>()
                .As<IImagePersistence>();
            builder.RegisterModule<LoggingModule>();
            builder.RegisterModule<WallrCoreModule>();
            builder.RegisterModule<WallrUiModule>();
            builder.RegisterType<OpenConfigPageOption>().As<IQuickUseOption>();
        }
    }
}