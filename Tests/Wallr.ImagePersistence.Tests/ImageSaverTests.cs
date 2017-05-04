using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using FakeItEasy;
using Microsoft.Reactive.Testing;
using Serilog;
using Shouldly;
using Wallr.ImageSource;
using Xunit;

namespace Wallr.ImagePersistence.Tests
{
    public class ImageSaverTests : IDisposable
    {
        private ImageSaver _sut;
        private IImageRepository _imageRepository;
        private TestScheduler _testScheduler;
        private IReadOnlyList<IImageFromSource> _imageFromSources;
        private IImageSources _imageSources;
        private IObservable<IImageFromSource> _imagesFromSourcesObservable;

        public ImageSaverTests()
        {
            _imageSources = A.Fake<IImageSources>();
            _imageFromSources = new[]
            {
                CreateImageFromSource("foo"),
                CreateImageFromSource("bar"),
                CreateImageFromSource("baz")
            };
            _testScheduler = new TestScheduler();
            _imagesFromSourcesObservable = _testScheduler.CreateHotObservable(
                _imageFromSources
                    .Select(
                        (image, i) => new Recorded<Notification<IImageFromSource>>(i + 2, // start events emitting after 2 ticks to allow for subscription
                            Notification.CreateOnNext(image)))
                    .ToArray());
            A.CallTo(() => _imageSources.ImagesFromAllSources).Returns(_imagesFromSourcesObservable);
            _imageRepository = A.Fake<IImageRepository>();
            _sut = new ImageSaver(_imageRepository, A.Fake<ILogger>());
        }

        [Fact]
        public void StartSavingImages_NoSubscriptionToSavedImages_SubscribesAndSavesImages()
        {
            var savedImages = _sut.StartSavingImages(_imageSources);
            _testScheduler.Start();

            foreach (IImageFromSource imageFromSource in _imageFromSources)
            {
                A.CallTo(() => _imageRepository.SaveImage(
                        A<SourceQualifiedImageId>.That.Matches(
                            id => id.ImageId.Equals(imageFromSource.Image.GetId().Result) &&
                                  id.SourceId.Equals(imageFromSource.SourceId)),
                        imageFromSource.Image.GetImageStream))
                    .MustHaveHappened();
            }
        }

        [Fact]
        public void Dispose_AfterStartedSavingImages_NoImagesSaved()
        {
            _sut.StartSavingImages(_imageSources);
            _sut.StartSavingImages(_imageSources); // subscribe twice to see that all subscriptions are disposed
            _sut.Dispose();
            _sut.Dispose(); // Check that disposal is idempotent
            _testScheduler.Start();

            A.CallTo(() => _imageRepository.SaveImage(A<SourceQualifiedImageId>._, A<Func<Task<Stream>>>._))
                .MustNotHaveHappened();
        }

        [Fact]
        public void StartSavingImages_SubscribedToSavedImages_SubscribesAndSavesImages()
        {
            IEnumerable<ISavedImage> expectedSavedImages = _imageFromSources.Select(_ => A.Fake<ISavedImage>()).ToList();
            foreach (var sourceAndSaved in _imageFromSources.Zip(expectedSavedImages, (source, saved) => new { Source = source, Saved = saved }))
            {
                A.CallTo(() => _imageRepository.SaveImage(
                        A<SourceQualifiedImageId>.That.Matches(
                            id => id.ImageId.Equals(sourceAndSaved.Source.Image.GetId().Result) &&
                                  id.SourceId.Equals(sourceAndSaved.Source.SourceId)),
                        sourceAndSaved.Source.Image.GetImageStream))
                    .Returns(sourceAndSaved.Saved);
            }

            ITestableObserver<ISavedImage> savedImages = _testScheduler.Start(() => _sut.StartSavingImages(_imageSources), 0, 0, 10);

            savedImages.Messages.Select(m => m.Value.Value).ShouldBe(expectedSavedImages);
        }

        public void Dispose()
        {
            _sut.Dispose();
        }

        private static IImageFromSource CreateImageFromSource(string imageId)
        {
            var imageFromSource = A.Fake<IImageFromSource>();
            IImage image = A.Fake<IImage>();
            A.CallTo(() => image.GetId()).Returns(new ImageId(imageId));
            A.CallTo(() => imageFromSource.SourceId).Returns(new ImageSourceId(Guid.NewGuid()));
            A.CallTo(() => imageFromSource.Image).Returns(image);
            return imageFromSource;
        }
    }
}