using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog;
using Wallr.ImagePersistence;
using Wallr.ImageQueue;
using Wallr.ImageSource;
using Wallr.Platform;
using Wallr.WallpaperUpdates;

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
        private readonly IImageSourceConfigurations _imageSourceConfigurations;
        private readonly IImageSaver _saver;
        private readonly IImageSources _imageSources;
        private readonly IWallpaperUpdater _wallpaperUpdater;

        public WallrApplication(ISetup setup, ILogger logger,
            IEnumerable<IQuickUseOption> quickUseOptions,
            IImageQueue imageQueue, IImageRepository imageRepository,
            IImageSourceConfigurations imageSourceConfigurations,
            IImageSaver saver, IImageSources imageSources,
            IWallpaperUpdater wallpaperUpdater)
        {
            _setup = setup;
            _logger = logger;
            _quickUseOptions = quickUseOptions;
            _imageQueue = imageQueue;
            _imageRepository = imageRepository;
            _imageSourceConfigurations = imageSourceConfigurations;
            _saver = saver;
            _imageSources = imageSources;
            _wallpaperUpdater = wallpaperUpdater;
        }

        public async Task Setup()
        {
            await _setup.SetupQuickUseControl(_quickUseOptions.ToList());
            await _imageQueue.Rehydrade(ids => ids.Select(_imageRepository.LoadImage));
            _imageQueue.StartQueuingSavedImages(_saver.StartSavingImages(_imageSources));
            await _imageSourceConfigurations.RehydrateSources();
            _wallpaperUpdater.UpdateWallpaperFrom(_imageQueue);
//            _wallpaperCoordinator.SubscribeToWallpaperUpdates();
            _logger.Information("Application started");
        }
    }
}
