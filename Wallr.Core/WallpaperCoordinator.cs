using System.Reactive.Linq;
using Wallr.ImageSource;
using Console = System.Console;
using System;

namespace Wallr.Core
{
    public class WallpaperCoordinator
    {
        private readonly IImageUpdateEvents _imageUpdateEvents;
        private readonly IWallpaperSetter _wallpaperSetter;
        private readonly IImageSource _imageSource;

        public WallpaperCoordinator(IImageUpdateEvents imageUpdateEvents, IWallpaperSetter wallpaperSetter, IImageSource imageSource)
        {
            _imageUpdateEvents = imageUpdateEvents;
            _wallpaperSetter = wallpaperSetter;
            _imageSource = imageSource;
        }

        public void Start()
        {
            _imageUpdateEvents.UpdateImageRequested
                .Select(i => _imageSource.FetchLatestImage())
                .Subscribe(_wallpaperSetter.SetWallpaper);
        } 
    }
}