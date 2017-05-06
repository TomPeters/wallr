using System;
using System.Collections.Generic;
using FluentAssertions;
using Wallr.ImagePersistence;
using Wallr.ImageQueue.Persistence;
using Wallr.ImageSource;
using Xunit;

namespace Wallr.ImageQueue.Tests
{
    public class ImageQueueSerializerTests
    {
        [Fact]
        public void RoundTrip_QueueValuesPreserved()
        {
            IEnumerable<SourceQualifiedImageId> queuedIds = new[]
            {
                CreateImageId("1"),
                CreateImageId("2"),
                CreateImageId("3")
            };

            var sut = new ImageQueueSerializer(new SourceQualifiedImageIdConverter());

            var json = sut.Serialize(queuedIds);
            sut.Deserialize(json).Should().Equal(queuedIds);
        }

        private SourceQualifiedImageId CreateImageId(string imageId)
        {
            return new SourceQualifiedImageId(new ImageSourceId(Guid.NewGuid()), new ImageId(imageId));
        }
    }
}