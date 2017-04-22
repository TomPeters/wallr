using System;

namespace Wallr.ImageSource
{
    public interface IImageSourceConfigurationFactory
    {
        ImageSourceConfiguration CreateNewSource(ImageSourceType sourceType);
    }

    public class ImageSourceConfigurationFactory : IImageSourceConfigurationFactory
    {
        public ImageSourceConfiguration CreateNewSource(ImageSourceType sourceType)
        {
            return new ImageSourceConfiguration(new ImageSourceId(Guid.NewGuid()),
                new ImageSourceName(sourceType.Value),
                sourceType,
                new ImageSourceSettings(), // nocommit, fix up default settings
                TimeSpan.FromDays(1), // nocommit, think about defaults
                true);
        }
    }
}