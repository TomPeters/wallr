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
        private readonly ILogger _logger;
        private readonly IEnumerable<IQuickUseOption> _quickUseOptions;

        public WallrApplication(IPlatform platform, ILogger logger, IEnumerable<IQuickUseOption> quickUseOptions)
        {
            _platform = platform;
            _logger = logger;
            _quickUseOptions = quickUseOptions;
        }

        public void Setup()
        {
            _platform.SetupQuickUseControl(_quickUseOptions.ToList());
//            _imageQueueCoordinator.UpdateImageQueuePeriodically();
//            _wallpaperCoordinator.SubscribeToWallpaperUpdates();
            _logger.Information("Application started");
        }
    }
}
