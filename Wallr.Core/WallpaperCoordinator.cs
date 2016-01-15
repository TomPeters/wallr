using System.Reactive.Linq;
using System;

namespace Wallr.Core
{
    public interface IWallpaperCoordinator
    {
        void Start();
    }

    public class WallpaperCoordinator : IWallpaperCoordinator
    {
        private readonly IWallpaperUpdateEvents _wallpaperUpdateEvents;
        private readonly IWallpaperSetter _wallpaperSetter;
        private readonly IImageStream _imageStream;

        public WallpaperCoordinator(IWallpaperUpdateEvents wallpaperUpdateEvents, IWallpaperSetter wallpaperSetter, IImageStream imageStream)
        {
            _wallpaperUpdateEvents = wallpaperUpdateEvents;
            _wallpaperSetter = wallpaperSetter;
            _imageStream = imageStream;
        }

        public void Start()
        {
            _wallpaperUpdateEvents.UpdateImageRequested
                .Where(i => _imageStream.ImageIds.Count > 0)
                .Select(i => _imageStream.PopNextImageId)
                .Subscribe(_wallpaperSetter.SetWallpaper);
        } 
    }
}