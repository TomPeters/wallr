using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Serilog;
using Serilog.Configuration;
using Wallr.Interfaces;
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

        public void SetupQuickUseControl(IReadOnlyList<IQuickUseOption> quickUseOptions)
        {
            IEnumerable<ToolStripItem> systemTrayOptions = quickUseOptions.Select(o => new ToolStripButton(o.Label, null, (sender, args) => o.SelectOption()));
            _applicationContext.InitializeNotifyIcon(systemTrayOptions);
        }

        private string ApplicationDataFolderPath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "wallr");

        public void SetWallpaper(ImageId imageId)
        {
            WindowsWallpapers.SetWallpaper(GetPathFromId(imageId));
            Log.Logger.ForContext("LocalImageId", imageId)
                .Information("Wallpaper set for {FileName}", imageId.LocalImageId.Value);
        }

        public void SaveWallpaper(IImage image)
        {
            var logger = Log.Logger.ForContext("LocalImageId", image.ImageId);
            logger.Information("Saving image {FileName}", image.ImageId.LocalImageId.Value);

            string path = GetPathFromId(image.ImageId);
            string directoryName = Path.GetDirectoryName(path);
            Directory.CreateDirectory(directoryName);
            using (Stream stream = image.FileStream)
            {
                Image img = Image.FromStream(stream);
                img.Save(path, System.Drawing.Imaging.ImageFormat.Jpeg);
            }

            logger.Information("Image saved at {FilePath}", path);
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