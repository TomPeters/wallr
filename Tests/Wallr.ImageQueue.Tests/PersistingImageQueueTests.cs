using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FakeItEasy;
using FluentAssertions;
using Optional;
using Serilog;
using Wallr.ImagePersistence;
using Wallr.ImageSource;
using Wallr.Platform;
using Xunit;

namespace Wallr.ImageQueue.Tests
{
    public class PersistingImageQueueTests
    {
        private readonly PersistingImageQueue _sut;
        private readonly IImageQueue _innerQueue;
        private readonly IPersistence _persistence;
        private readonly IEnumerable<SourceQualifiedImageId> _enqueuedImageIds;
        private Func<SourceQualifiedImageId, ISavedImage> _fetchSavedImages;
        private readonly IImageQueueSerializer _serializer;
        private const string ImageQueuePersistenceKey = "ImageQueue";
        private const string EnqueuedImageIdsJson = "Some Json";

        public PersistingImageQueueTests()
        {
            _enqueuedImageIds = new[]
            {
                CreateImageId("1"),
                CreateImageId("2"),
                CreateImageId("3")
            };
            _innerQueue = A.Fake<IImageQueue>();
            A.CallTo(() => _innerQueue.QueuedImageIds).Returns(_enqueuedImageIds);
            _persistence = A.Fake<IPersistence>();
            _serializer = A.Fake<IImageQueueSerializer>();
            A.CallTo(() => _serializer.Serialize(A<IEnumerable<SourceQualifiedImageId>>.That.IsSameSequenceAs(_enqueuedImageIds))).Returns(EnqueuedImageIdsJson);
            _sut = new PersistingImageQueue(_persistence, _serializer, _innerQueue, A.Fake<ILogger>());
        }

        [Fact]
        public async Task Enqueue_PersistsImageQueueAfterEnqueuing()
        {
            var imagesToEnqueue = new [] {A.Dummy<ISavedImage>()};
            await _sut.Enqueue(imagesToEnqueue);

            A.CallTo(() => _innerQueue.Enqueue(A<IEnumerable<ISavedImage>>.That.IsSameSequenceAs(imagesToEnqueue)))
                .MustHaveHappened(Repeated.Exactly.Once)
                .Then(A.CallTo(() => _persistence.SaveSettings(ImageQueuePersistenceKey, EnqueuedImageIdsJson)).MustHaveHappened());
        }

        [Fact]
        public async Task Dequeue_PersistsImageQueueAfterDequeuing()
        {
            Option<ISavedImage> expectedDequeuedImage = A.Dummy<ISavedImage>().Some();
            A.CallTo(() => _innerQueue.Dequeue()).Returns(expectedDequeuedImage);

            Option<ISavedImage> dequeuedImage = await _sut.Dequeue();

            dequeuedImage.Should().Be(expectedDequeuedImage);
            A.CallTo(() => _innerQueue.Dequeue()).MustHaveHappened()
                .Then(A.CallTo(() => _persistence.SaveSettings(ImageQueuePersistenceKey, EnqueuedImageIdsJson)).MustHaveHappened());
        }

        [Fact]
        public async Task Clear_PersistsImageQueueAfterClearing()
        {
            await _sut.Clear();

            A.CallTo(() => _innerQueue.Clear())
                .MustHaveHappened(Repeated.Exactly.Once)
                .Then(A.CallTo(() => _persistence.SaveSettings(ImageQueuePersistenceKey, EnqueuedImageIdsJson)).MustHaveHappened());
        }

        [Fact]
        public void QueuedImageIds_ReturnsIdsFromInnerQueue()
        {
            _sut.QueuedImageIds.Should().Equal(_enqueuedImageIds);
        }

        [Fact]
        public async Task Rehydrate_SettingsExist_ClearsQueueThenEnqueuesImages()
        {
            const string someJson = "Some persisted Json";
            A.CallTo(() => _persistence.LoadSettings(ImageQueuePersistenceKey)).Returns(someJson.Some());
            var imageIds = new[]
            {
                CreateImageId("1"),
                CreateImageId("2"),
                CreateImageId("3")
            };
            A.CallTo(() => _serializer.Deserialize(someJson)).Returns(imageIds);
            var savedImages = new[]
            {
                A.Dummy<ISavedImage>(),
                A.Dummy<ISavedImage>(),
                A.Dummy<ISavedImage>()
            };
            _fetchSavedImages = A.Fake<Func<SourceQualifiedImageId, ISavedImage>>();
            A.CallTo(() => _fetchSavedImages(imageIds[0])).Returns(savedImages[0]);
            A.CallTo(() => _fetchSavedImages(imageIds[1])).Returns(savedImages[1]);
            A.CallTo(() => _fetchSavedImages(imageIds[2])).Returns(savedImages[2]);

            await _sut.Rehydrade(_fetchSavedImages);

            A.CallTo(() => _innerQueue.Clear()).MustHaveHappened()
                .Then(A.CallTo(() => _innerQueue.Enqueue(A<IEnumerable<ISavedImage>>.That.IsSameSequenceAs(savedImages))).MustHaveHappened());
        }

        [Fact]
        public async Task Rehydrate_SettingsDontExist_ClearsQueue()
        {
            A.CallTo(() => _persistence.LoadSettings(ImageQueuePersistenceKey)).Returns(Option.None<string>());

            await _sut.Rehydrade(A.Fake<Func<SourceQualifiedImageId, ISavedImage>>());

            A.CallTo(() => _innerQueue.Clear()).MustNotHaveHappened();
            A.CallTo(() => _innerQueue.Enqueue(A<IEnumerable<ISavedImage>>._)).MustNotHaveHappened();
        }

        private static SourceQualifiedImageId CreateImageId(string imageId)
        {
            return new SourceQualifiedImageId(new ImageSourceId(Guid.NewGuid()), new ImageId(imageId));
        }
    }
}