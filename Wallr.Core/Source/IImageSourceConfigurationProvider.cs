using System.Collections.Generic;

namespace Wallr.Core.Source
{
    public interface IImageSourceConfigurationProvider
    {
        IEnumerable<IImageSourceConfiguration> ImageSourceConfigurations { get; } // nocommit implement
    }

    public class ImageSourceConfigurationProvider : IImageSourceConfigurationProvider
    {
        public IEnumerable<IImageSourceConfiguration> ImageSourceConfigurations
        {
            get
            {
                // nocommit, implement properly
                yield return new ImageSourceConfiguration(new ConfiguredImageSourceId(), new ImageSourceType("Some fake key"), new Dictionary<string, string>());
            }
        }
    }
}