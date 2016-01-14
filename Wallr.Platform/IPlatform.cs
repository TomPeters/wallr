using System;
using System.Collections.Generic;
using Serilog;
using Serilog.Configuration;
using Wallr.Interfaces;

namespace Wallr.Platform
{
    public interface IPlatform // TODO: interface segregation, add extra abstraction on top of this in core where required (eg image saving)
    {
        void SetupQuickUseControl(IReadOnlyList<IQuickUseOption> quickUseOptions);
        void SetWallpaper(ImageId imageId);
        void SaveWallpaper(IImage image);
        IEnumerable<Func<LoggerSinkConfiguration, LoggerConfiguration>> LoggerSinks { get; }
    }
}