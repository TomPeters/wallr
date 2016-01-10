using System.IO;
using Wallr.ImageSource;

namespace Wallr.Core
{
    public interface IWallpaperSetter
    {
        void SetWallpaper(IImage image);
    }

    public class WallpaperSetter : IWallpaperSetter
    {
        private readonly IPlatform _platform;

        public WallpaperSetter(IPlatform platform)
        {
            _platform = platform;
        }

        public void SetWallpaper(IImage image)
        {
            SaveWallpaper(image);
            _platform.SetWallpaper(image.StreamImageId);
        }

        private void SaveWallpaper(IImage image)
        {
            using (Stream stream = image.FileStream)
            {
                _platform.SaveWallpaper(stream, image.StreamImageId);
            }
        }
    }
}