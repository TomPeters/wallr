using System.Collections.Generic;

namespace Wallr.Core.Source
{
    public interface IImageSourceConfiguration
    {
        ConfiguredImageSourceId Id { get; }
        ImageSourceTypeKey ImageSourceType { get; }
        IDictionary<string, string> Settings { get; } 
    }

    public class ImageSourceConfiguration : IImageSourceConfiguration
    {
        public ImageSourceConfiguration(ConfiguredImageSourceId id, ImageSourceTypeKey imageSourceType, IDictionary<string, string> settings)
        {
            Id = id;
            ImageSourceType = imageSourceType;
            Settings = settings;
        }

        public ConfiguredImageSourceId Id { get; }
        public ImageSourceTypeKey ImageSourceType { get; }
        public IDictionary<string, string> Settings { get; }
    }

    public class ImageSourceConfigurationFactory
    {
        public IImageSourceConfiguration CreateConfiguredImageSource(ImageSourceTypeKey imageSourceTypeKey)
        {
            return new ImageSourceConfiguration(new ConfiguredImageSourceId(), imageSourceTypeKey, new Dictionary<string, string>());
        }
    }
}