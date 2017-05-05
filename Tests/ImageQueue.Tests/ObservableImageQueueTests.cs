using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading.Tasks;
using FakeItEasy;
using FluentAssertions;
using Microsoft.Reactive.Testing;
using Optional;
using Wallr.ImagePersistence;
using Wallr.ImageQueue;
using Xunit;

namespace ImageQueue.Tests
{
    public class ObservableImageQueueTests
    {
        private readonly IPersistingImageQueue _innerQueue;
        private readonly ObservableImageQueue _sut;
        private readonly TestScheduler _testScheduler;

        public ObservableImageQueueTests()
        {
            _testScheduler = new TestScheduler();
            _innerQueue = A.Fake<IPersistingImageQueue>();
            _sut = new ObservableImageQueue(_innerQueue);
        }

        [Fact]
        public void Enqueue_ImageQueueChangesEventFired()
        {
            var savedImages = new[] {A.Dummy<ISavedImage>()};

            _testScheduler.ScheduleAsync(TimeSpan.FromTicks(5), (s, t) => _sut.Enqueue(savedImages));
            ITestableObserver<ImageQueueChangedEvent> imageQueueChanges = _testScheduler.Start(() => _sut.ImageQueueChanges, 0, 0, 100);

            imageQueueChanges.Messages.Should().HaveCount(1);
            A.CallTo(() => _innerQueue.Enqueue(savedImages)).MustHaveHappened();
        }

        [Fact]
        public void Rehydrate_ImageQueueChangesEventFired()
        {
            var fetchSavedImages = A.Fake<Func<SourceQualifiedImageId, ISavedImage>>();

            _testScheduler.ScheduleAsync(TimeSpan.FromTicks(5), (s, t) => _sut.Rehydrade(fetchSavedImages));
            ITestableObserver<ImageQueueChangedEvent> imageQueueChanges = _testScheduler.Start(() => _sut.ImageQueueChanges, 0, 0, 100);

            imageQueueChanges.Messages.Should().HaveCount(1);
            A.CallTo(() => _innerQueue.Rehydrade(fetchSavedImages)).MustHaveHappened();
        }

        [Fact]
        public void Dequeue_ImageWasDequeuedFromInnerQueue_ImageQueueChangesEventFired()
        {
            Option<ISavedImage> expectedDequeuedImage = A.Dummy<ISavedImage>().Some();
            A.CallTo(() => _innerQueue.Dequeue()).Returns(expectedDequeuedImage);

            Option<ISavedImage> dequeuedImage = Option.None<ISavedImage>();
            _testScheduler.ScheduleAsync(TimeSpan.FromTicks(5), async (s, t) =>
            {
                dequeuedImage = await _sut.Dequeue();
            });
            ITestableObserver<ImageQueueChangedEvent> imageQueueChanges = _testScheduler.Start(() => _sut.ImageQueueChanges, 0, 0, 100);

            imageQueueChanges.Messages.Should().HaveCount(1);
            dequeuedImage.HasValue.Should().BeTrue();
            dequeuedImage.Should().Be(expectedDequeuedImage);
        }

        [Fact]
        public void Dequeue_ImageWasNotDequeuedFromInnerQueue_ImageQueueChangesEventNotFired()
        {
            Option<ISavedImage> expectedDequeuedImage = Option.None<ISavedImage>();
            A.CallTo(() => _innerQueue.Dequeue()).Returns(Option.None<ISavedImage>());

            Option<ISavedImage> dequeuedImage = A.Dummy<ISavedImage>().Some();
            _testScheduler.ScheduleAsync(TimeSpan.FromTicks(5), async (s, t) =>
            {
                dequeuedImage = await _sut.Dequeue();
            });
            ITestableObserver<ImageQueueChangedEvent> imageQueueChanges = _testScheduler.Start(() => _sut.ImageQueueChanges, 0, 0, 100);

            imageQueueChanges.Messages.Should().BeEmpty();
            dequeuedImage.HasValue.Should().BeFalse();
            dequeuedImage.Should().Be(expectedDequeuedImage);
        }

        [Fact]
        public void Clear_ImageQueueChangesEventFired()
        {
            _testScheduler.ScheduleAsync(TimeSpan.FromTicks(5), (s, t) => _sut.Clear());
            ITestableObserver<ImageQueueChangedEvent> imageQueueChanges = _testScheduler.Start(() => _sut.ImageQueueChanges, 0, 0, 100);

            imageQueueChanges.Messages.Should().HaveCount(1);
            A.CallTo(() => _innerQueue.Clear()).MustHaveHappened();
        }

        [Fact]
        public void QueuedImageIds_ReturnsIdsFromInnerQueue()
        {
            var enqueuedImageIds = new List<SourceQualifiedImageId>();
            A.CallTo(() => _innerQueue.QueuedImageIds).Returns(enqueuedImageIds);
            _sut.QueuedImageIds.Should().BeSameAs(enqueuedImageIds);
        }

        [Fact]
        public void StartQueueingSavedImages_ImagesAreEnqueued()
        {
            var image1 = A.Dummy<ISavedImage>();
            var image2 = A.Dummy<ISavedImage>();
            var image3 = A.Dummy<ISavedImage>();
            var savedImages = _testScheduler.CreateHotObservable(
                new Recorded<Notification<ISavedImage>>(10, Notification.CreateOnNext(image1)),
                new Recorded<Notification<ISavedImage>>(20, Notification.CreateOnNext(image2)),
                new Recorded<Notification<ISavedImage>>(30, Notification.CreateOnNext(image3)));

            _testScheduler.Schedule(TimeSpan.FromTicks(5), (s, t) => _sut.StartQueuingSavedImages(savedImages));

            var imageQueueChangesCallback = A.Fake<Action<ImageQueueChangedEvent>>();
            _sut.ImageQueueChanges.Subscribe(imageQueueChangesCallback);
            ITestableObserver<ImageQueueChangedEvent> imageQueueChanges = _testScheduler.Start(() => _sut.ImageQueueChanges, 0, 0, 100);

            A.CallTo(() => _innerQueue.Enqueue(A<IEnumerable<ISavedImage>>.That.IsSameSequenceAs(new[] {image1}))).MustHaveHappened()
                .Then(A.CallTo(() => imageQueueChangesCallback(A<ImageQueueChangedEvent>._)).MustHaveHappened())
                .Then(A.CallTo(() => _innerQueue.Enqueue(A<IEnumerable<ISavedImage>>.That.IsSameSequenceAs(new[] {image2}))).MustHaveHappened())
                .Then(A.CallTo(() => imageQueueChangesCallback(A<ImageQueueChangedEvent>._)).MustHaveHappened())
                .Then(A.CallTo(() => _innerQueue.Enqueue(A<IEnumerable<ISavedImage>>.That.IsSameSequenceAs(new[] {image3}))).MustHaveHappened())
                .Then(A.CallTo(() => imageQueueChangesCallback(A<ImageQueueChangedEvent>._)).MustHaveHappened());


            imageQueueChanges.Messages.Should().HaveCount(3);
        }
    }
}