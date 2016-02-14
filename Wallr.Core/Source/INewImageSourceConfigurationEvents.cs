using System;

namespace Wallr.Core.Source
{
    public interface INewImageSourceConfigurationEvents
    {
        IObservable<NewImageSourceConfigurationEvent> Events { get; }
    }

    public class NewImageSourceConfigurationEvent
    {
        public ImageSourceType SourceType { get; }

        public NewImageSourceConfigurationEvent(ImageSourceType imageSourceType)
        {
            SourceType = imageSourceType;
        }
    }
}