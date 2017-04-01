using System;
using Wallr.ImagePersistence;

namespace Wallr.UI.Models
{
    public class ImageIdViewModel
    {
        private readonly SourceQualifiedImageId _sourceQualifiedImageId;

        public ImageIdViewModel(SourceQualifiedImageId sourceQualifiedImageId)
        {
            _sourceQualifiedImageId = sourceQualifiedImageId;
        }

        public Guid ImageSourceId => _sourceQualifiedImageId.SourceId.Value;
        public string LocalImageId => _sourceQualifiedImageId.ImageId.Value;
    }
}