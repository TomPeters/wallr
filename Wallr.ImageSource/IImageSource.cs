using System;
using System.Reactive.Linq;

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

    public class ImageSourceFactory
    {
        private readonly IImageSourcePluginFactory _imageSourcePluginFactory;

        public ImageSourceFactory(IImageSourcePluginFactory imageSourcePluginFactory)
        {
            _imageSourcePluginFactory = imageSourcePluginFactory;
        }

        public IImageSource CreateImageSource(ImageSourceConfiguration imageSourceConfiguration)
        {
            return new ImageSource(imageSourceConfiguration, _imageSourcePluginFactory);
        }
    }

    public class ImageSource : IImageSource
    {
        private readonly ImageSourceConfiguration _configuration;
        private readonly IImageSourcePluginFactory _imageSourcePluginFactory;

        public ImageSource(ImageSourceConfiguration configuration, IImageSourcePluginFactory imageSourcePluginFactory)
        {
            _configuration = configuration;
            _imageSourcePluginFactory = imageSourcePluginFactory;
        }

        public ImageSourceId ImageSourceId => _configuration.ImageSourceId;

        public IObservable<IImage> Images
        {
            get
            {
                return Observable.Interval(_configuration.UpdateInterval)
                    .Select(_ => _imageSourcePluginFactory.CreateImageSourcePlugin(_configuration.SourceType)
                        .GetImages(_configuration.Settings))
                    .SelectMany(e => e.ToObservable());
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