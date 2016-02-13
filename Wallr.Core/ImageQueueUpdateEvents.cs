using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Serilog;

namespace Wallr.Core
{
    public interface IImageQueueUpdateEvents
    {
        IObservable<int> ImageQueueUpdateRequested { get; }
    }

    public class ImageQueueUpdateEvents : IImageQueueUpdateEvents
    {
        private readonly ILogger _logger;

        public ImageQueueUpdateEvents(ILogger logger)
        {
            var manualEvents = new Subject<long>();
            _logger = logger;
            int numberOfImagesToAdd = 10;
            ImageQueueUpdateRequested = Observable.Interval(new TimeSpan(0, 0, 0, 10)).Merge(manualEvents).Replay().RefCount().Select(i => numberOfImagesToAdd);
            ImageQueueUpdateRequested.Subscribe(l => _logger.Information("{NumberOfImagesRequested} images requested from source at {Timestamp}", numberOfImagesToAdd, DateTime.UtcNow));
            manualEvents.OnNext(0);
        }

        public IObservable<int> ImageQueueUpdateRequested { get; }
    }
}