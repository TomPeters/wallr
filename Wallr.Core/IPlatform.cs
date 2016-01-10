using System;
using System.Collections.Generic;
using System.IO;
using Serilog;
using Serilog.Configuration;
using Wallr.Core.QuickUse;
using Wallr.ImageSource;

namespace Wallr.Core
{
    public interface IPlatform
    {
        void SetupQuickUseControl(IReadOnlyList<IQuickUseOption> quickUseOptions);
        void SetWallpaper(ImageId imageId);
        void SaveWallpaper(Stream fileStream, ImageId imageId);
        IEnumerable<Func<LoggerSinkConfiguration, LoggerConfiguration>> LoggerSinks { get; }
    }
}