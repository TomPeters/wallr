using Autofac;
using Wallr.ImagePersistence;
using Wallr.ImageQueue;
using Wallr.ImageSource;
using Wallr.Platform;
using Wallr.UI;
using Wallr.WallpaperUpdates;

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
            builder.RegisterModule<ImagePersistenceModule>();
            builder.RegisterModule<ImageQueueModule>();
            builder.RegisterModule<ImageSourceModule>();
            builder.RegisterModule<WallpaperUpdatesModule>();
            builder.RegisterModule<WallrUiModule>();
            builder.RegisterType<OpenConfigPageOption>().As<IQuickUseOption>();
            builder.RegisterType<WallrApplication>().As<IWallrApplication>();
        }
    }
}