using System.Collections.Generic;
using System.IO;
using Wallr.Core.QuickUse;

namespace Wallr.Core
{
    public interface IPlatform
    {
        void SetupQuickUseControl(IReadOnlyList<IQuickUseOption> quickUseOptions);
        string ApplicationDataFolderPath { get; } // FIXME: Ideally we should not need to expose this, look for ways to avoid it.
        void SetWallpaper(string imagePath);
        void SaveWallpaper(Stream fileStream, string filePath);
    }
}