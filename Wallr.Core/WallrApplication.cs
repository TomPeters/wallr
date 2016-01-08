using System;
using System.Collections.Generic;
using System.IO;
using Serilog;
using Wallr.Core.QuickUse;

namespace Wallr.Core
{
    public class WallrApplication
    {
        public void Setup(IPlatform platform)
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.WithProperty("InstanceId", Guid.NewGuid())
                .MinimumLevel.Debug()
                .WriteTo.ColoredConsole()
                .WriteTo.File(Path.Combine(platform.ApplicationDataFolderPath, "log.txt"))
                .CreateLogger();
            platform.SetupQuickUseControl(new List<IQuickUseOption>());
        }
    }
}
