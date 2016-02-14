using System.Collections.Generic;
using Wallr.Common;
using Wallr.ImageSource;

namespace Wallr.Core
{
    public interface IConfiguredImageSource // nocommit use this instead of iimagesource
    {
        ConfiguredImageSourceId Id { get; }
        ImageSourceTypeKey ImageSourceType { get; }
        IEnumerable<ISourceImage> Images { get; } 
    }

    public class ImageSourceTypeKey : TypedString<ImageSourceTypeKey>
    {
        public ImageSourceTypeKey(string value) : base(value)
        {
        }
    }
}