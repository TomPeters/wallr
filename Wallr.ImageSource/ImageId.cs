using Wallr.Common;

namespace Wallr.ImageSource
{
    public class ImageId : TypedString<ImageId>
    {
        public ImageId(string value) : base(value)
        {
        }
    }

    public class ImageSourceId : TypedString<ImageSourceId>
    {
        public ImageSourceId(string value) : base(value)
        {
        }
    }

    public class StreamImageId
    {
        public StreamImageId(ImageId imageId, ImageSourceId imageSourceId)
        {
            ImageId = imageId;
            ImageSourceId = imageSourceId;
        }

        public ImageId ImageId { get;}
        public ImageSourceId ImageSourceId { get; }
    }
}