using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FakeItEasy;
using FluentAssertions;
using Optional;
using Optional.Unsafe;
using Serilog;
using Wallr.ImagePersistence;
using Wallr.ImageSource;
using Xunit;

namespace Wallr.ImageQueue.Tests
{
    public class ImageQueueTests
    {
        private Wallr.ImageQueue.ImageQueue _sut;

        public ImageQueueTests()
        {
            _sut = new Wallr.ImageQueue.ImageQueue(A.Fake<ILogger>());
        }

        [Fact]
        public async Task Enqueue_ImagesAddedToQueuedImageIds()
        {
            ImageIds().Should().BeEmpty();
            await _sut.Enqueue(new[] {CreateSavedImage("1")});

            ImageIds().Should().Equal("1");

            await _sut.Enqueue(new[] {CreateSavedImage("2"), CreateSavedImage("3")});

            ImageIds().Should().Equal("1", "2", "3");
        }

        [Fact]
        public async Task Dequeue_HasNothingInQueue_ReturnsEmpty()
        {
            Option<ISavedImage> dequeued = await _sut.Dequeue();
            dequeued.HasValue.Should().BeFalse();
        }

        [Fact]
        public async Task Dequeue_HasItemsInQueue_PullsFirstItemFromQueue()
        {
            ISavedImage firstImageInQueue = CreateSavedImage("1");
            await _sut.Enqueue(new[] {firstImageInQueue, CreateSavedImage("2"), CreateSavedImage("3")});

            var dequeued = await _sut.Dequeue();

            ImageIds().Should().Equal("2", "3");
            dequeued.HasValue.Should().BeTrue();
            dequeued.ValueOrFailure().Should().Be(firstImageInQueue);
        }

        [Fact]
        public async Task Clear_QueueBecomesEmpty()
        {
            await _sut.Enqueue(new[] {CreateSavedImage("1"), CreateSavedImage("2"), CreateSavedImage("3")});

            await _sut.Clear();

            ImageIds().Should().BeEmpty();
        }

        private IEnumerable<string> ImageIds()
        {
            return _sut.QueuedImageIds.Select(i => i.ImageId.Value);
        }

        private ISavedImage CreateSavedImage(string imageId)
        {
            var savedImage = A.Fake<ISavedImage>();
            A.CallTo(() => savedImage.Id).Returns(new SourceQualifiedImageId(new ImageSourceId(Guid.NewGuid()), new ImageId(imageId)));
            return savedImage;
        }

    }
}