using System;
using System.Collections.Generic;
using System.Linq;
using Serilog;
using Wallr.Core.QuickUse;
using Wallr.ImageSource.Subreddit;

namespace Wallr.Core
{
    public class WallrApplication
    {
        public void Setup(IPlatform platform)
        {
            LoggerConfiguration loggerConfiguration = new LoggerConfiguration()
                .Enrich.WithProperty("InstanceId", Guid.NewGuid())
                .MinimumLevel.Debug();
            Log.Logger = platform.LoggerSinks
                .Aggregate(loggerConfiguration, (current, sinkConfiguration) => sinkConfiguration(current.WriteTo))
                .CreateLogger();

            platform.SetupQuickUseControl(new List<IQuickUseOption>());
            new WallpaperCoordinator(new ImageUpdateEvents(Log.Logger), new WallpaperSetter(platform), new SubredditImageSource("wallpapers", Log.Logger)).Start();
        }
    }
}
