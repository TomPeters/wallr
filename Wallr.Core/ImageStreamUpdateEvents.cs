using System;
using System.Reactive.Linq;
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
            _logger = logger;
            int numberOfImagesToAdd = 30;
            ImageStreamUpdateRequested = Observable.Interval(new TimeSpan(0, 0, 0, 10)).Publish().RefCount().Select(i => numberOfImagesToAdd);
            ImageStreamUpdateRequested.Subscribe(l => _logger.Information("{NumberOfImagesRequested} images requested from source at {Timestamp}", numberOfImagesToAdd, DateTime.UtcNow));
        }

        public IObservable<int> ImageStreamUpdateRequested { get; }
    }
}