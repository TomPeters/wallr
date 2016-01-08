using System;
using System.Reactive.Linq;
using Serilog;

namespace Wallr.Core
{
    public interface IImageUpdateEvents
    {
        IObservable<long> UpdateImageRequested { get; }
    }

    public class ImageUpdateEvents : IImageUpdateEvents
    {
        private readonly ILogger _logger;

        public ImageUpdateEvents(ILogger logger)
        {
            _logger = logger;
            UpdateImageRequested = Observable.Interval(new TimeSpan(0, 0, 0, 10)).Publish().RefCount();
            UpdateImageRequested.Subscribe(l => _logger.Information("Image update requested at {Timestamp}", DateTime.UtcNow));
        }

        public IObservable<long> UpdateImageRequested { get; }
    }
}