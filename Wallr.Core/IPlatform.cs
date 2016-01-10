using System;
using System.Collections.Generic;
using System.IO;
using Serilog;
using Serilog.Configuration;
using Wallr.Core.QuickUse;
using Wallr.ImageSource;

namespace Wallr.Core
{
    public interface IPlatform // TODO: interface segregation, add extra abstraction on top of this in core where required (eg image saving)
    {
        void SetupQuickUseControl(IReadOnlyList<IQuickUseOption> quickUseOptions);
        void SetWallpaper(ImageId imageId);
        void SaveWallpaper(IImage image);
        IEnumerable<Func<LoggerSinkConfiguration, LoggerConfiguration>> LoggerSinks { get; }
    }
}