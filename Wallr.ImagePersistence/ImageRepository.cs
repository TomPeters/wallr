using System;
using System.IO;
using System.Threading.Tasks;
using Optional;
using Serilog;
using Wallr.Platform;

namespace Wallr.ImagePersistence
{
    public interface IImageRepository
    {
        Task<ISavedImage> SaveImage(SourceQualifiedImageId id, Func<Task<Stream>> getImageStream);
        ISavedImage LoadImage(SourceQualifiedImageId id);
    }

    public class ImageRepository : IImageRepository
    {
        private readonly IImagePersistence _imagePersistence;
        private readonly ILogger _logger;

        public ImageRepository(IImagePersistence imagePersistence, ILogger logger)
        {
            _imagePersistence = imagePersistence;
            _logger = logger;
        }

        public async Task<ISavedImage> SaveImage(SourceQualifiedImageId id, Func<Task<Stream>> getImageStream)
        {
            await _imagePersistence.SaveImage(id.SourceId.Value, id.ImageId.Value, getImageStream, _logger);
            return LoadImage(id);
        }

        public ISavedImage LoadImage(SourceQualifiedImageId id)
        {
            return new SavedImage(id, _imagePersistence);
        }

        private class SavedImage : ISavedImage
        {
            private readonly IImagePersistence _imagePersistence;

            public SavedImage(SourceQualifiedImageId id, IImagePersistence imagePersistence)
            {
                _imagePersistence = imagePersistence;
                Id = id;
            }

            public SourceQualifiedImageId Id { get; }
            public Task<Option<Stream>> GetStream()
            {
                return _imagePersistence.LoadImage(Id.SourceId.Value, Id.ImageId.Value);
            }
        }
    }
}