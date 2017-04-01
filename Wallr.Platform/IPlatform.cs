using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Optional;
using Serilog;
using Serilog.Configuration;
using Wallr.ImagePersistence;

namespace Wallr.Platform
{
    public interface IConfiguration
    {
        IEnumerable<Func<LoggerSinkConfiguration, LoggerConfiguration>> LoggerSinks { get; }
    }

    public interface ISetup
    {
        Task SetupQuickUseControl(IReadOnlyList<IQuickUseOption> quickUseOptions); // nocommit, Task
    }

    public interface INavigation
    {
        Task NavigateToUrl(string url); // nocommit, task
    }

    public interface IWallpaperEnvironment
    {
        Task SetWallpaper(SourceQualifiedImageId sourceQualifiedImageId, ILogger logger); // nocommit, Task
    }

    public interface IPersistence
    {
        Task SaveSettings(string settingsKey, string settings); // nocommit used to be void
        Task<Option<string>> LoadSettings(string settingsKey); // nocommit, used to be Maybe<string>
    }

    public interface IPlatform : IConfiguration, IImagePersistence, INavigation, ISetup, IWallpaperEnvironment, IPersistence
    {
    }
}