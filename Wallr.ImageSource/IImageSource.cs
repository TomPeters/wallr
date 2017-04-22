using System;
using System.Collections.Async;
using System.Threading.Tasks;

namespace Wallr.ImageSource
{
    public interface IImageSource // nocommit, implementation delegates to ImageSourceConfiguration
    {
        ImageSourceId ImageSourceId { get; }
        IObservable<IImage> Images { get; }
    }

    public interface IImageSourceSettings
    {
        // nocommit: visitor? for different settings types
        // nocommit, some way of updating the settings??
    }

    public class ImageSource : IImageSource
    {
        private readonly ImageSourceConfiguration _configuration;

        public ImageSource(ImageSourceConfiguration configuration)
        {
            _configuration = configuration;
        }

        public ImageSourceId ImageSourceId => _configuration.ImageSourceId;

        public IObservable<IImage> Images
        {
            get
            {
                return null;
                // nocommit, do something interesting here :)
            }
        }
    }

    public class ImageSourceConfiguration
    {
        public ImageSourceConfiguration(ImageSourceId imageSourceId,
            ImageSourceName imageSourceName,
            ImageSourceType sourceType,
            IImageSourceSettings settings,
            TimeSpan updateInterval)
        {
            ImageSourceId = imageSourceId;
            ImageSourceName = imageSourceName;
            SourceType = sourceType;
            Settings = settings;
            UpdateInterval = updateInterval;
        }

        public ImageSourceId ImageSourceId { get; }
        public ImageSourceName ImageSourceName { get; }
        public ImageSourceType SourceType { get; }
        public IImageSourceSettings Settings { get; }
        public TimeSpan UpdateInterval { get; }
        public bool Enabled { get; }
    }
}