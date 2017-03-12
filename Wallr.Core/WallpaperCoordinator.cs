using System.Reactive.Linq;
using System;

namespace Wallr.Core
{
    public interface IWallpaperCoordinator
    {
        void SubscribeToWallpaperUpdates();
    }

    public class WallpaperCoordinator : IWallpaperCoordinator
    {
        private readonly IWallpaperUpdateEvents _wallpaperUpdateEvents;
        private readonly IWallpaperSetter _wallpaperSetter;
        private readonly IImageQueue _imageQueue;

        public WallpaperCoordinator(IWallpaperUpdateEvents wallpaperUpdateEvents, IWallpaperSetter wallpaperSetter, IImageQueue imageQueue)
        {
            _wallpaperUpdateEvents = wallpaperUpdateEvents;
            _wallpaperSetter = wallpaperSetter;
            _imageQueue = imageQueue;
        }

        public void SubscribeToWallpaperUpdates()
        {
            _wallpaperUpdateEvents.UpdateImageRequested
                .Where(i => _imageQueue.ImageIds.Count > 0)
                .Select(i => _imageQueue.PopNextImageId)
                .Subscribe(_wallpaperSetter.SetWallpaper);
        } 
    }
}