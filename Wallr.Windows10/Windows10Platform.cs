using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Serilog;
using Serilog.Configuration;
using Wallr.Platform;

namespace Wallr.Windows10
{
    public class Windows10Platform : IPlatform
    {
        private readonly WallrApplicationContext _applicationContext;

        public Windows10Platform(WallrApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }

        public void NavigateToUrl(string url)
        {
            Process.Start(url);
        }

        public void SetupQuickUseControl(IReadOnlyList<IQuickUseOption> quickUseOptions)
        {
            IEnumerable<ToolStripItem> systemTrayOptions = quickUseOptions.Select(o => new ToolStripButton(o.Label, null, (sender, args) => o.SelectOption()));
            _applicationContext.InitializeNotifyIcon(systemTrayOptions);
        }

        private string ApplicationDataFolderPath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "wallr");

        public void SetWallpaper(ImageId imageId, ILogger logger)
        {
            WindowsWallpapers.SetWallpaper(GetPathFromId(imageId));
            logger.ForContext("LocalImageId", imageId)
                .Information("Wallpaper set for {FileName}", imageId.LocalImageId.Value);
        }

        public void SaveWallpaper(IImage image, ILogger logger)
        {
            var contextLogger = logger.ForContext("LocalImageId", image.ImageId);
            contextLogger.Information("Saving image {FileName}", image.ImageId.LocalImageId.Value);

            string path = GetPathFromId(image.ImageId);
            string directoryName = Path.GetDirectoryName(path);
            Directory.CreateDirectory(directoryName);
            using (Stream stream = image.FileStream)
            {
                Image img = Image.FromStream(stream);
                img.Save(path, System.Drawing.Imaging.ImageFormat.Jpeg);
            }

            contextLogger.Information("Image saved at {FilePath}", path);
        }

        public Stream LoadImage(ImageId imageId)
        {
            return File.OpenRead(GetPathFromId(imageId));
        }

        public IEnumerable<Func<LoggerSinkConfiguration, LoggerConfiguration>> LoggerSinks
        {
            get
            {
                yield return writeTo => writeTo.ColoredConsole();
                yield return writeTo => writeTo.File(Path.Combine(ApplicationDataFolderPath, "log.txt"));
                yield return writeTo => writeTo.Seq("http://localhost:5341/");
            }
        }

        private string GetPathFromId(ImageId imageId)
        {
            return Path.Combine(ApplicationDataFolderPath, imageId.ImageSourceId.Value, $"{imageId.LocalImageId.Value}.jpg");
        }

        public void Start(IDisposable applicationExitDependency)
        {
            Application.ApplicationExit += (sender, args) => applicationExitDependency.Dispose();
            Application.Run(_applicationContext);
        }
    }
}