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
                .Enrich.WithProperty("Product", "Wallr")
                .Enrich.WithProperty("InstanceId", Guid.NewGuid())
                .MinimumLevel.Debug();
            Log.Logger = platform.LoggerSinks
                .Aggregate(loggerConfiguration, (current, sinkConfiguration) => sinkConfiguration(current.WriteTo))
                .CreateLogger();

            platform.SetupQuickUseControl(new List<IQuickUseOption>());
            ImageStream imageStream = new ImageStream(platform, Log.Logger);
            var subredditImageSource = new SubredditImageSource("wallpapers", Log.Logger);
            var populator = new ImageStreamPopulator(subredditImageSource, Log.Logger);
            new ImageStreamCoordinator(populator, new ImageStreamUpdateEvents(Log.Logger), imageStream).Start();
            new WallpaperCoordinator(new WallpaperUpdateEvents(Log.Logger), new WallpaperSetter(platform), imageStream).Start();
        }
    }
}
