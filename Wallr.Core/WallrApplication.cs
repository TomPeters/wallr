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
        private readonly IImageStreamCoordinator _imageStreamCoordinator;
        private readonly IWallpaperCoordinator _wallpaperCoordinator;
        private readonly ILogger _logger;

        public WallrApplication(IPlatform platform, IImageStreamCoordinator imageStreamCoordinator,
            IWallpaperCoordinator wallpaperCoordinator, ILogger logger)
        {
            _platform = platform;
            _imageStreamCoordinator = imageStreamCoordinator;
            _wallpaperCoordinator = wallpaperCoordinator;
            _logger = logger;
        }

        public void Setup()
        {
            _platform.SetupQuickUseControl(new List<IQuickUseOption>());
            _imageStreamCoordinator.Start();
            _wallpaperCoordinator.Start();
            _logger.Information("Application started");
        }
    }
}
