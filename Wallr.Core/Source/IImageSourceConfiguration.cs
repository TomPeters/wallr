using System.Collections.Generic;

namespace Wallr.Core.Source
{
    public interface IImageSourceConfiguration
    {
        ConfiguredImageSourceId Id { get; }
        ImageSourceType ImageSourceType { get; }
        IDictionary<string, string> Settings { get; } 
    }

    public class ImageSourceConfiguration : IImageSourceConfiguration
    {
        public ImageSourceConfiguration(ConfiguredImageSourceId id, ImageSourceType imageSourceType, IDictionary<string, string> settings)
        {
            Id = id;
            ImageSourceType = imageSourceType;
            Settings = settings;
        }

        public ConfiguredImageSourceId Id { get; }
        public ImageSourceType ImageSourceType { get; }
        public IDictionary<string, string> Settings { get; }
    }

    public class ImageSourceConfigurationFactory
    {
        public IImageSourceConfiguration CreateConfiguredImageSource(ImageSourceType imageSourceType)
        {
            return new ImageSourceConfiguration(new ConfiguredImageSourceId(), imageSourceType, new Dictionary<string, string>());
        }
    }
}