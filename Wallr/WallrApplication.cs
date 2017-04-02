using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog;
using Wallr.ImagePersistence;
using Wallr.ImageQueue;
using Wallr.Platform;

namespace Wallr
{
    public interface IWallrApplication
    {
        Task Setup();
    }

    public class WallrApplication : IWallrApplication
    {
        private readonly ISetup _setup;
        private readonly ILogger _logger;
        private readonly IEnumerable<IQuickUseOption> _quickUseOptions;
        private readonly IImageQueue _imageQueue;
        private readonly IImageRepository _imageRepository;

        public WallrApplication(ISetup setup, ILogger logger,
            IEnumerable<IQuickUseOption> quickUseOptions,
            IImageQueue imageQueue, IImageRepository imageRepository)
        {
            _setup = setup;
            _logger = logger;
            _quickUseOptions = quickUseOptions;
            _imageQueue = imageQueue;
            _imageRepository = imageRepository;
        }

        public async Task Setup()
        {
            await _setup.SetupQuickUseControl(_quickUseOptions.ToList());
            await _imageQueue.Rehydrade(ids => ids.Select(_imageRepository.LoadImage));
//            _imageQueueCoordinator.UpdateImageQueuePeriodically();
//            _wallpaperCoordinator.SubscribeToWallpaperUpdates();
            _logger.Information("Application started");
        }
    }
}
