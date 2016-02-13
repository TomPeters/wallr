using System.Collections.Generic;
using Serilog;
using Wallr.Platform;

namespace Wallr.Core
{
    public interface IWallrApplication
    {
        void Setup();
    }

    public class WallrApplication : IWallrApplication
    {
        private readonly IPlatform _platform;
        private readonly IImageQueueCoordinator _imageQueueCoordinator;
        private readonly IWallpaperCoordinator _wallpaperCoordinator;
        private readonly ILogger _logger;

        public WallrApplication(IPlatform platform, IImageQueueCoordinator imageQueueCoordinator,
            IWallpaperCoordinator wallpaperCoordinator, ILogger logger)
        {
            _platform = platform;
            _imageQueueCoordinator = imageQueueCoordinator;
            _wallpaperCoordinator = wallpaperCoordinator;
            _logger = logger;
        }

        public void Setup()
        {
            _platform.SetupQuickUseControl(new List<IQuickUseOption>());
            _imageQueueCoordinator.Start();
            _wallpaperCoordinator.Start();
            _logger.Information("Application started");
        }
    }
}
