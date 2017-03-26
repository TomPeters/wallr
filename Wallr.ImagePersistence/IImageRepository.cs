using System.IO;
using System.Threading.Tasks;
using Optional;
using Serilog;

namespace Wallr.ImagePersistence
{
    public interface IImagePersistence // Move to Wallr.Platform? Maybe just leave it here...
    {
        void SaveImage(ImageId imageId, ILogger logger);
        Task<Option<Stream>> LoadImage(ImageId imageId);
    }

    public interface IImageRepository
    {
        ISavedImage SaveImage(ImageId id);
        Task<Option<Stream>> LoadImage(ImageId id);
    }

    public interface ISavedImage
    {
        ImageId Id { get; }
        Task<Option<Stream>> GetStream();
    }

    // nocommit, implement
    public class ImageId // ImageId is an ID that identifies a persisted image, and therefore belongs in this assembly. It should be used primarily by the image persistence and the image queue, which can be thought of as a queue of persisted images.
    {

    }
}