using System;
using System.IO;
using System.Threading.Tasks;
using FakeItEasy;
using FluentAssertions;
using Optional;
using Serilog;
using Wallr.ImageSource;
using Wallr.Platform;
using Xunit;

namespace Wallr.ImagePersistence.Tests
{
    public class ImageRepositoryTests
    {
        private readonly ImageRepository _imageRepository;
        private readonly IImagePersistence _imagePersistence;
        private readonly SourceQualifiedImageId _imageId;
        private readonly Option<Stream> _imageStreamFromPersistence;

        public ImageRepositoryTests()
        {
            _imageId = new SourceQualifiedImageId(new ImageSourceId(Guid.NewGuid()), new ImageId("some image id"));
            _imagePersistence = A.Fake<IImagePersistence>();
            _imageStreamFromPersistence = new MemoryStream().Some<Stream>();
            A.CallTo(() => _imagePersistence.LoadImage(_imageId.SourceId.Value, _imageId.ImageId.Value)).Returns(_imageStreamFromPersistence);
            _imageRepository = new ImageRepository(_imagePersistence, A.Fake<ILogger>());
        }

        [Fact]
        public async Task SaveImage_PersistsImage_ReturnsImageThatCanBeFetchedFromPersistence()
        {
            var getImageStream = A.Fake<Func<Task<Stream>>>();
            ISavedImage savedImage = await _imageRepository.SaveImage(_imageId, getImageStream);

            A.CallTo(() => _imagePersistence.SaveImage(_imageId.SourceId.Value, _imageId.ImageId.Value, getImageStream, A<ILogger>._))
                .MustHaveHappened(Repeated.Exactly.Once);

            savedImage.Id.Should().Be(_imageId);
            (await savedImage.GetStream()).Should().Be(_imageStreamFromPersistence);
        }

        [Fact]
        public async Task LoadImage_ReturnsImageThatCanBeFetchedFromPersistence()
        {
            var savedImage = _imageRepository.LoadImage(_imageId);
            savedImage.Id.Should().Be(_imageId);
            (await savedImage.GetStream()).Should().Be(_imageStreamFromPersistence);
        }
    }
}