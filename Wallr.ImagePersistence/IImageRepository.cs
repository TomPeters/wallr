using System.IO;
using System.Threading.Tasks;
using Optional;
using Serilog;
using Wallr.ImageSource;

namespace Wallr.ImagePersistence
{
    // nocommit, needs an implementation
    public interface IImagePersistence // Move to Wallr.Platform? Maybe just leave it here...
    {
        Task SaveImage(ImageId imageId, Stream imageStream, ILogger logger);
        Task<Option<Stream>> LoadImage(ImageId imageId);
    }

    public interface IImageRepository // nocommit, needs an implementation
    {
        Task<ISavedImage> SaveImage(ImageId id);
        Task<Option<Stream>> LoadImage(ImageId id);
    }

    public interface ISavedImage
    {
        ImageId Id { get; }
        Task<Option<Stream>> GetStream();
    }

    // This id that identifies a persisted image, and therefore belongs in this assembly.
    // It should be used primarily by the image persistence and the image queue, which can be thought of as a queue of persisted images.
    public class ImageId
    {
        public ImageId(ImageSourceId sourceId, ImageSource.ImageId sourceImageId)
        {
            SourceId = sourceId;
            SourceImageId = sourceImageId;
        }

        public ImageSourceId SourceId { get; }
        public ImageSource.ImageId SourceImageId { get; }
    }
}