using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Serilog;

namespace Wallr.Core
{
    public interface IImageStreamUpdateEvents
    {
        IObservable<int> ImageStreamUpdateRequested { get; }
    }

    public class ImageStreamUpdateEvents : IImageStreamUpdateEvents
    {
        private readonly ILogger _logger;

        public ImageStreamUpdateEvents(ILogger logger)
        {
            var manualEvents = new Subject<long>();
            _logger = logger;
            int numberOfImagesToAdd = 300;
            ImageStreamUpdateRequested = Observable.Interval(new TimeSpan(0, 1, 0, 0)).Merge(manualEvents).Replay().RefCount().Select(i => numberOfImagesToAdd);
            ImageStreamUpdateRequested.Subscribe(l => _logger.Information("{NumberOfImagesRequested} images requested from source at {Timestamp}", numberOfImagesToAdd, DateTime.UtcNow));
            manualEvents.OnNext(0);
        }

        public IObservable<int> ImageStreamUpdateRequested { get; }
    }
}