using Wallr.ImageSource;

namespace Wallr.ImagePersistence
{
    public class SourceQualifiedImageId
    {
        public SourceQualifiedImageId(ImageSourceId sourceId, ImageId imageId)
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