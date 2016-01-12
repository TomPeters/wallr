using System;
using System.Linq;
using System.Reactive.Linq;
using Serilog;

namespace Wallr.Core
{
    public interface IWallpaperUpdateEvents
    {
        IObservable<long> UpdateImageRequested { get; }
    }

    public class WallpaperUpdateEvents : IWallpaperUpdateEvents
    {
        private readonly ILogger _logger;

        public WallpaperUpdateEvents(ILogger logger)
        {
            _logger = logger;
            // TODO: This won't cut it because the stream should not be interupted by restarting the application
            UpdateImageRequested = Observable.Interval(new TimeSpan(0, 0, 30, 0)).Publish().RefCount();
            UpdateImageRequested.Subscribe(l => _logger.Information("Image update requested at {Timestamp}", DateTime.UtcNow));
        }

        public IObservable<long> UpdateImageRequested { get; }
    }
}