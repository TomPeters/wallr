using Wallr.Platform;

namespace Wallr.UI.Models
{
    public class ImageIdViewModel
    {
        private readonly ImageId _imageId;

        public ImageIdViewModel(ImageId imageId)
        {
            _imageId = imageId;
        }

        public string ImageSourceId => _imageId.ImageSourceId.Value;
        public string LocalImageId => _imageId.LocalImageId.Value;
    }
}