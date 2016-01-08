using System.IO;
using Serilog;
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
        private readonly ILogger _logger;

        public WallpaperSetter(IPlatform platform, ILogger logger)
        {
            _platform = platform;
            _logger = logger;
        }

        public void SetWallpaper(IImage image)
        {
            var logger = _logger.ForContext("ImageId", image.Id);
            string path = SaveWallpaper(image, logger);
            logger.Information("Image saved at {FilePath}", path);
            _platform.SetWallpaper(path);
            logger.Information("Wallpaper set for {FileName}", image.FileName);
        }

        private string SaveWallpaper(IImage image, ILogger logger)
        {
            logger.Information("Saving image {FileName}", image.FileName);
            using (Stream stream = image.FileStream)
            {
                var filePath = Path.Combine(_platform.ApplicationDataFolderPath, image.FileName);
                _platform.SaveWallpaper(stream, filePath);
                return filePath;
            }
        }
    }
}