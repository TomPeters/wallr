using Wallr.ImageSource;

namespace Wallr.Core
{
    public interface IWallpaperSetter
    {
        void SetWallpaper(ImageId imageId);
    }

    public class WallpaperSetter : IWallpaperSetter
    {
        private readonly IPlatform _platform;

        public WallpaperSetter(IPlatform platform)
        {
            _platform = platform;
        }

        public void SetWallpaper(ImageId imageId)
        {
            _platform.SetWallpaper(imageId);
        }
    }
}