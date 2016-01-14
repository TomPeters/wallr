using System;

namespace Wallr.Interfaces
{
    public class ImageId : IEquatable<ImageId>
    {
        public ImageId(LocalImageId localImageId, ImageSourceId imageSourceId)
        {
            LocalImageId = localImageId;
            ImageSourceId = imageSourceId;
        }

        public LocalImageId LocalImageId { get; }
        public ImageSourceId ImageSourceId { get; }

        public bool Equals(ImageId other)
        {
            return other.LocalImageId.Equals(LocalImageId) && other.ImageSourceId.Equals(ImageSourceId);
        }

        public override bool Equals(object obj)
        {
            return obj is ImageId && Equals((ImageId)obj);
        }

        public override int GetHashCode()
        {
            return LocalImageId.GetHashCode() ^ ImageSourceId.GetHashCode();
        }
    }
}