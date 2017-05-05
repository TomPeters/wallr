using System;

namespace Wallr.ImageQueue
{
    public interface IImageQueueEvents
    {
        IObservable<ImageQueueChangedEvent> ImageQueueChanges { get; }
    }

    public class ImageQueueChangedEvent
    {
    }
}