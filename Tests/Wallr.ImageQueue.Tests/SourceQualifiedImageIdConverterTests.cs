using System;
using FluentAssertions;
using Wallr.ImagePersistence;
using Wallr.ImageQueue.Persistence;
using Wallr.ImageSource;
using Xunit;

namespace Wallr.ImageQueue.Tests
{
    public class SourceQualifiedImageIdConverterTests
    {
        [Fact]
        public void RoundTrip_ValuesAreEquivalent()
        {
            var sut = new SourceQualifiedImageIdConverter();
            var id = new SourceQualifiedImageId(new ImageSourceId(Guid.NewGuid()), new ImageId("some image id"));
            var serializationModel = sut.ToSerializationModel(id);
            sut.FromSerializationModel(serializationModel).Should().Be(id);
        }
    }
}