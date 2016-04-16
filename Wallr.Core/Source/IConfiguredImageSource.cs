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
        private readonly IStatefulImageSource _statefulImageSource;

        public ConfiguredImageSource(ConfiguredImageSourceId id, IStatefulImageSource statefulImageSource)
        {
            _statefulImageSource = statefulImageSource;
            Id = id;
        }

        public ConfiguredImageSourceId Id { get; }
        public IEnumerable<ISourceImage> Images => _statefulImageSource.Images;
    }

    public interface IImageSourcesProvider // nocommit implementation
    {
        IEnumerable<IStatefulImageSource> AvailableImageSources { get; }
    }
}