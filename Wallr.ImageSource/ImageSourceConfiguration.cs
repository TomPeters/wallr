using System;

namespace Wallr.ImageSource
{
    public interface IImageSourceConfiguration
    {
        ImageSourceId ImageSourceId { get; }
        ImageSourceName ImageSourceName { get; }
        ImageSourceType SourceType { get; }
        IImageSourceSettings Settings { get; }
        TimeSpan UpdateInterval { get; }
        bool IsEnabled { get; }
    }

    public class ImageSourceConfiguration : IImageSourceConfiguration
    {
        public ImageSourceConfiguration(ImageSourceId imageSourceId,
            ImageSourceName imageSourceName,
            ImageSourceType sourceType,
            IImageSourceSettings settings,
            TimeSpan updateInterval,
            bool isEnabled)
        {
            ImageSourceId = imageSourceId;
            ImageSourceName = imageSourceName;
            SourceType = sourceType;
            Settings = settings;
            UpdateInterval = updateInterval;
            IsEnabled = isEnabled;
        }

        public ImageSourceId ImageSourceId { get; }
        public ImageSourceName ImageSourceName { get; }
        public ImageSourceType SourceType { get; }
        public IImageSourceSettings Settings { get; }
        public TimeSpan UpdateInterval { get; }
        public bool IsEnabled { get; }
    }
}