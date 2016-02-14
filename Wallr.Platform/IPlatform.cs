using System;
using System.Collections.Generic;
using System.IO;
using Serilog;
using Serilog.Configuration;
using Wallr.Common;
using Wallr.Interfaces;

namespace Wallr.Platform
{
    public interface IPlatform // TODO: interface segregation, add extra abstraction on top of this in core where required (eg image saving)
    {
        void NavigateToUrl(string url);
        void SetupQuickUseControl(IReadOnlyList<IQuickUseOption> quickUseOptions);
        void SetWallpaper(ImageId imageId, ILogger logger);
        // TODO: Will need a LoadWallpaper at some point
        void SaveWallpaper(IImage image, ILogger logger);
        Stream LoadImage(ImageId imageId);
        IEnumerable<Func<LoggerSinkConfiguration, LoggerConfiguration>> LoggerSinks { get; }
        void SaveSettings(string settingsKey, string settings);
        IMaybe<string> LoadSettings(string settingsKey);
    }
}