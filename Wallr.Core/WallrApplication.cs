using System.Collections.Generic;
using System.Linq;
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
        private readonly IEnumerable<IQuickUseOption> _quickUseOptions;

        public WallrApplication(IPlatform platform, IImageQueueCoordinator imageQueueCoordinator,
            IWallpaperCoordinator wallpaperCoordinator, ILogger logger, IEnumerable<IQuickUseOption> quickUseOptions)
        {
            _platform = platform;
            _imageQueueCoordinator = imageQueueCoordinator;
            _wallpaperCoordinator = wallpaperCoordinator;
            _logger = logger;
            _quickUseOptions = quickUseOptions;
        }

        public void Setup()
        {
            _platform.SetupQuickUseControl(_quickUseOptions.ToList());
            _imageQueueCoordinator.Start();
            _wallpaperCoordinator.Start();
            _logger.Information("Application started");
        }
    }
}
