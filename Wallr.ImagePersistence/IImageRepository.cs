using System;
using System.IO;
using System.Threading.Tasks;
using Optional;
using Serilog;
using Wallr.ImageSource;

namespace Wallr.ImagePersistence
{
    public interface IImagePersistence
    {
        Task SaveImage(SourceQualifiedImageId sourceQualifiedImageId, Func<Stream> createImageStream, ILogger logger);
        Task<Option<Stream>> LoadImage(SourceQualifiedImageId sourceQualifiedImageId);
    }

    public interface IImageRepository // nocommit, needs an implementation
    {
        Task<ISavedImage> SaveImage(SourceQualifiedImageId id);
        Task<Option<Stream>> LoadImage(SourceQualifiedImageId id);
    }

    public interface ISavedImage
    {
        SourceQualifiedImageId Id { get; }
        Task<Option<Stream>> GetStream();
    }

    // This id that identifies a persisted image, and therefore belongs in this assembly.
    // It should be used primarily by the image persistence and the image queue, which can be thought of as a queue of persisted images.
    public class SourceQualifiedImageId
    {
        public SourceQualifiedImageId(ImageSourceId sourceId, ImageSource.ImageId imageId)
        {
            SourceId = sourceId;
            ImageId = imageId;
        }

        public ImageSourceId SourceId { get; }
        public ImageId ImageId { get; }

        protected bool Equals(SourceQualifiedImageId other)
        {
            return Equals(SourceId, other.SourceId) && Equals(ImageId, other.ImageId);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((SourceQualifiedImageId) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((SourceId != null ? SourceId.GetHashCode() : 0) * 397) ^ (ImageId != null ? ImageId.GetHashCode() : 0);
            }
        }
    }
}