using Wallr.Common;

namespace Wallr.ImageSource
{
    // Used for the filename on disk. 
    // Should correspond closely to the actual file name if possible (minus the extension). 
    // Can have things like timestamps if required for uniqueness
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