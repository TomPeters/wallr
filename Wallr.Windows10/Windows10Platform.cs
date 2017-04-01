using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Nito.AsyncEx;
using Optional;
using Serilog;
using Serilog.Configuration;
using Wallr.ImagePersistence;
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

        public Task NavigateToUrl(string url)
        {
            return Task.Run(() => Process.Start(url));
        }

        public Task SetupQuickUseControl(IReadOnlyList<IQuickUseOption> quickUseOptions)
        {
            IEnumerable<ToolStripItem> systemTrayOptions = quickUseOptions.Select(o => new ToolStripButton(o.Label, null, async (sender, args) => await o.SelectOption())); // be aware that this is an async void EventHandler
            _applicationContext.InitializeNotifyIcon(systemTrayOptions); // Can't wrap this in a Task.Run, because it needs to run on the main thread
            return TaskConstants.Completed;
        }

        private string ApplicationDataFolderPath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "wallr");

        public async Task SetWallpaper(ImageId imageId, ILogger logger)
        {
            await Task.Run(() => WindowsWallpapers.SetWallpaper(GetPathFromId(imageId)));
            logger.ForContext("LocalImageId", imageId)
                .Information("Wallpaper set for {FileName}", imageId.SourceImageId.Value);
        }

        public async Task SaveImage(ImageId imageId, Func<Stream> createImageStream, ILogger logger)
        {
            var contextLogger = logger.ForContext("LocalImageId", imageId);
            contextLogger.Information("Saving image {FileName}", imageId.SourceImageId.Value);

            string path = GetPathFromId(imageId);
            string directoryName = Path.GetDirectoryName(path);
            await Task.Run(() => Directory.CreateDirectory(directoryName));
            using (Stream stream = createImageStream())
            {
                Image img = await Task.Run(() => Image.FromStream(stream));
                await Task.Run(() => img.Save(path, System.Drawing.Imaging.ImageFormat.Jpeg));
            }

            contextLogger.Information("Image saved at {FilePath}", path);
        }

        public Task<Option<Stream>> LoadImage(ImageId imageId)
        {
            string path = GetPathFromId(imageId);
            if (!File.Exists(path)) return Task.FromResult(Option.None<Stream>());
            return Task.FromResult(
                new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, FileOptions.Asynchronous)
                    .Some<Stream>()
            );
        }

        public IEnumerable<Func<LoggerSinkConfiguration, LoggerConfiguration>> LoggerSinks
        {
            get
            {
                yield return writeTo => writeTo.LiterateConsole();
                yield return writeTo => writeTo.File(Path.Combine(ApplicationDataFolderPath, "log.txt"));
                yield return writeTo => writeTo.Seq("http://localhost:5341/");
            }
        }

        public async Task SaveSettings(string settingsKey, string settings)
        {
            using (var streamWriter = new StreamWriter(Path.Combine(ApplicationDataFolderPath, settingsKey)))
                await streamWriter.WriteAsync(settings);
        }

        public async Task<Option<string>> LoadSettings(string settingsKey)
        {
            string path = Path.Combine(ApplicationDataFolderPath, settingsKey);
            if (!File.Exists(path)) return Option.None<string>();

            using (var reader = new StreamReader(path))
                return (await reader.ReadToEndAsync()).Some();
        }

        private string GetPathFromId(ImageId imageId)
        {
            return Path.Combine(ApplicationDataFolderPath, imageId.SourceId.Value.ToString(), $"{imageId.SourceImageId.Value}.jpg");
        }

        public void Start(IDisposable applicationExitDependency)
        {
            Application.ApplicationExit += (sender, args) => applicationExitDependency.Dispose();
            Application.Run(_applicationContext);
        }
    }
}