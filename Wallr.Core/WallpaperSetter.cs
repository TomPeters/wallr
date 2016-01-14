using System;
using Serilog;
using Wallr.Interfaces;
using Wallr.Platform;

namespace Wallr.Core
{
    public interface IWallpaperSetter
    {
        void SetWallpaper(ImageId imageId);
    }

    public class WallpaperSetter : IWallpaperSetter
    {
        private readonly IPlatform _platform;
        private readonly ILogger _logger;

        public WallpaperSetter(IPlatform platform, ILogger logger)
        {
            _platform = platform;
            _logger = logger;
        }

        public void SetWallpaper(ImageId imageId)
        {
            try
            {
                _platform.SetWallpaper(imageId);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "An error ocurred trying to set the wallpaper to {ImageId}", imageId);
            }
        }
    }
}