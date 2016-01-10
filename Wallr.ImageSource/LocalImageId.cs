using Wallr.Common;

namespace Wallr.ImageSource
{
    // Used for the filename on disk. 
    // Should correspond closely to the actual file name if possible (minus the extension). 
    // Can have things like timestamps if required for uniqueness
    // Only needs to be unique within the scope of an image source
    public class LocalImageId : TypedString<LocalImageId>
    {
        public LocalImageId(string value) : base(value)
        {
        }
    }

    public class ImageSourceId : TypedString<ImageSourceId>
    {
        public ImageSourceId(string value) : base(value)
        {
        }
    }

    public class ImageId
    {
        public ImageId(LocalImageId localImageId, ImageSourceId imageSourceId)
        {
            LocalImageId = localImageId;
            ImageSourceId = imageSourceId;
        }

        public LocalImageId LocalImageId { get;}
        public ImageSourceId ImageSourceId { get; }
    }
}