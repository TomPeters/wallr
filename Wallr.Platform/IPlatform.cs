using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Optional;
using Serilog;
using Serilog.Configuration;

namespace Wallr.Platform
{
    public interface IConfiguration
    {
        IEnumerable<Func<LoggerSinkConfiguration, LoggerConfiguration>> LoggerSinks { get; }
    }

    public interface ISetup
    {
        Task SetupQuickUseControl(IReadOnlyList<IQuickUseOption> quickUseOptions);
    }

    public interface INavigation
    {
        Task NavigateToUrl(string url);
    }

    public interface IWallpaperEnvironment
    {
        Task SetWallpaper(Guid sourceId, string imageId, ILogger logger);
    }

    public interface IPersistence
    {
        Task SaveSettings(string settingsKey, string settings);
        Task<Option<string>> LoadSettings(string settingsKey);
    }

    public interface IPlatform : IConfiguration, IImagePersistence, INavigation, ISetup, IWallpaperEnvironment, IPersistence
    {
    }
}