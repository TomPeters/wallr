using System.Collections.Generic;
using Wallr.ImageSource;

namespace Wallr.Core.Source
{
    public interface IConfiguredImageSource // nocommit use this instead of iimagesource
    {
        ConfiguredImageSourceId Id { get; }
        IEnumerable<ISourceImage> Images { get; }
    }

    public class ConfiguredImageSource : IConfiguredImageSource
    {
        private readonly IImageSource _imageSource;

        public ConfiguredImageSource(ConfiguredImageSourceId id, IImageSource imageSource)
        {
            _imageSource = imageSource;
            Id = id;
        }

        public ConfiguredImageSourceId Id { get; }
        public IEnumerable<ISourceImage> Images => _imageSource.Images;
    }

    public interface IImageSourcesProvider // nocommit implementation
    {
        IEnumerable<IImageSource> AvailableImageSources { get; }
    }
}