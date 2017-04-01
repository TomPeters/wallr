using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Optional;
using Serilog;
using Serilog.Configuration;
using Wallr.Common;
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

    public interface IImagePersistence // nocommit, move to Wallr.ImagePersistence
    {
        Task SaveImage(ImageId imageId, Func<Stream> createImageStream, ILogger logger);
        // nocommit used to be void
        // used to be         void SaveWallpaper(IImage image, ILogger logger);
        Task<Option<Stream>> LoadImage(ImageId imageId);
        // nocommit used to be void
        // used to be         Stream LoadImage(ImageId imageId);
    }

    public interface IWallpaperEnvironment
    {
        Task SetWallpaper(ImageId imageId, ILogger logger); // nocommit, Task
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