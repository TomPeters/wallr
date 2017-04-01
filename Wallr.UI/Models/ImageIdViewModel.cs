using System;
using Wallr.ImagePersistence;

namespace Wallr.UI.Models
{
    public class ImageIdViewModel
    {
        private readonly ImageId _imageId;

        public ImageIdViewModel(ImageId imageId)
        {
            _imageId = imageId;
        }

        public Guid ImageSourceId => _imageId.SourceId.Value;
        public string LocalImageId => _imageId.SourceImageId.Value;
    }
}