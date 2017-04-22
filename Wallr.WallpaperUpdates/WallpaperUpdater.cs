using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Runtime.InteropServices;
using Serilog;
using Wallr.ImageQueue;
using Wallr.Platform;

namespace Wallr.WallpaperUpdates
{
    public interface IWallpaperUpdater
    {
        void UpdateWallpaperFrom(IImageQueue imageQueue);
    }

    public class WallpaperUpdater : IDisposable, IWallpaperUpdater
    {
        private readonly IWallpaperEnvironment _wallpaperEnvironment;
        private readonly ILogger _logger;
        private readonly List<IDisposable> _wallpaperUpdateSubscriptions = new List<IDisposable>();

        public WallpaperUpdater(IWallpaperEnvironment wallpaperEnvironment, ILogger logger)
        {
            _wallpaperEnvironment = wallpaperEnvironment;
            _logger = logger;
        }

        public void UpdateWallpaperFrom(IImageQueue imageQueue)
        {
            _wallpaperUpdateSubscriptions.Add(Observable.Interval(TimeSpan.FromSeconds(10)) // TODO: Make this a configurable setting
                .SelectMany(i => Observable.FromAsync(_ => imageQueue.Dequeue()))
                .Select(i => Observable.FromAsync(_ => _wallpaperEnvironment.SetWallpaper(i.Id.SourceId.Value, i.Id.ImageId.Value, _logger)))
                .Subscribe());
        }

        public void Dispose()
        {
            foreach(IDisposable subscription in _wallpaperUpdateSubscriptions)
                subscription.Dispose();
        }
    }
}