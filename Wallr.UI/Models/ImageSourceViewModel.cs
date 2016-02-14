using System;
using System.Collections.Generic;
using Wallr.Core.Source;

namespace Wallr.UI.Models
{
    public class ImageSourceViewModel
    {
        private readonly IImageSourceConfiguration _imageSourceConfiguration;

        public ImageSourceViewModel(IImageSourceConfiguration imageSourceConfiguration)
        {
            _imageSourceConfiguration = imageSourceConfiguration;
        }

        public Guid Id => _imageSourceConfiguration.Id.Value;
        public string Type => _imageSourceConfiguration.ImageSourceType.Value;
        public IDictionary<string, string> Settings => _imageSourceConfiguration.Settings;
    }
}