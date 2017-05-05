using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FakeItEasy;
using Serilog;
using Wallr.ImagePersistence;
using Wallr.ImageQueue;
using Wallr.ImageSource;
using Wallr.Platform;
using Wallr.WallpaperUpdates;
using Xunit;

namespace Wallr.Tests
{
    public class WallrApplicationTests
    {
        private readonly ISetup _setup;
        private readonly IEnumerable<IQuickUseOption> _quickUseOptions;
        private readonly IObservableImageQueue _imageQueue;
        private readonly IImageSourceConfigurations _imageSourceConfigurations;
        private readonly IWallpaperUpdater _wallpaperUpdater;
        private readonly WallrApplication _wallrApplication;
        private readonly IObservable<ISavedImage> _savedImages;
        private IImageRepository _imageRepository;

        public WallrApplicationTests()
        {
            _setup = A.Fake<ISetup>();
            _quickUseOptions = new[] {A.Fake<IQuickUseOption>(), A.Fake<IQuickUseOption>(), A.Fake<IQuickUseOption>() };
            _imageQueue = A.Fake<IObservableImageQueue>();
            _imageRepository = A.Fake<IImageRepository>();
            _imageSourceConfigurations = A.Fake<IImageSourceConfigurations>();
            var imageSaver = A.Fake<IImageSaver>();
            var imageSources = A.Fake<IImageSources>();
            _savedImages = A.Fake<IObservable<ISavedImage>>();
            A.CallTo(() => imageSaver.StartSavingImages(imageSources)).Returns(_savedImages);
            _wallpaperUpdater = A.Fake<IWallpaperUpdater>();
            var testingSources = A.Fake<TestingSources>();
            _wallrApplication = new WallrApplication(_setup, A.Fake<ILogger>(), _quickUseOptions, _imageQueue,
                _imageRepository, _imageSourceConfigurations, imageSaver, imageSources, _wallpaperUpdater, testingSources);
        }

        [Fact]
        public async Task Setup_Called_InitializedServices()
        {
            await _wallrApplication.Setup();

            A.CallTo(() => _setup.SetupQuickUseControl(A<IReadOnlyList<IQuickUseOption>>.That.IsSameSequenceAs(_quickUseOptions)))
                .MustHaveHappened(Repeated.Exactly.Once)
                .Then(A.CallTo(() => _imageQueue.Rehydrade(_imageRepository.LoadImage))
                    .MustHaveHappened(Repeated.Exactly.Once))
                .Then(A.CallTo(() => _imageQueue.StartQueuingSavedImages(_savedImages))
                    .MustHaveHappened(Repeated.Exactly.Once))
                .Then(A.CallTo(() => _imageSourceConfigurations.RehydrateSources())
                    .MustHaveHappened(Repeated.Exactly.Once))
                .Then(A.CallTo(() => _wallpaperUpdater.UpdateWallpaperFrom(_imageQueue))
                    .MustHaveHappened(Repeated.Exactly.Once));
        }
    }
}