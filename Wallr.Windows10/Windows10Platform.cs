using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Wallr.Core;
using Wallr.Core.QuickUse;

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

        public string ApplicationDataFolderPath
        {
            get { return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "wallr"); }
        }

        public void SetWallpaper(string imagePath)
        {
            WindowsWallpapers.SetWallpaper(imagePath);
        }

        public void SaveWallpaper(Stream fileStream, string filePath)
        {
            Image img = Image.FromStream(fileStream);
            img.Save(filePath, System.Drawing.Imaging.ImageFormat.Jpeg);
        }

        public void Start()
        {
            Application.Run(_applicationContext);
        }
    }
}