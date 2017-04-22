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
        private readonly TestingSources _testingSources;

        public WallrApplication(ISetup setup, ILogger logger,
            IEnumerable<IQuickUseOption> quickUseOptions,
            IImageQueue imageQueue, IImageRepository imageRepository,
            IImageSourceConfigurations imageSourceConfigurations,
            IImageSaver saver, IImageSources imageSources,
            IWallpaperUpdater wallpaperUpdater,
            TestingSources testingSources)
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
            _testingSources = testingSources;
        }

        public async Task Setup()
        {
            await _setup.SetupQuickUseControl(_quickUseOptions.ToList());
            _logger.Information("Setup quick use controls");
            await _imageQueue.Rehydrade(ids => ids.Select(_imageRepository.LoadImage));
            _logger.Information("Image queue loaded from persistence");
            _imageQueue.StartQueuingSavedImages(_saver.StartSavingImages(_imageSources));
            _logger.Information("Image queue connecting to image sources");
            await _imageSourceConfigurations.RehydrateSources();
            _logger.Information("Image sources loaded from persistence");
            _wallpaperUpdater.UpdateWallpaperFrom(_imageQueue);
            _logger.Information("Wallpaper updates connected to image queue");
            foreach (IImageSourceConfiguration source in _testingSources.GetTestingSourceConfigurations())
                await _imageSourceConfigurations.Add(source);
            _logger.Information("Application started");
        }
    }
}
