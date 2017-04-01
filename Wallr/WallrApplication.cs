﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog;
using Wallr.Platform;

namespace Wallr
{
    public interface IWallrApplication
    {
        Task Setup();
    }

    public class WallrApplication : IWallrApplication
    {
        private readonly ISetup _setup;
        private readonly ILogger _logger;
        private readonly IEnumerable<IQuickUseOption> _quickUseOptions;

        public WallrApplication(ISetup setup, ILogger logger, IEnumerable<IQuickUseOption> quickUseOptions)
        {
            _setup = setup;
            _logger = logger;
            _quickUseOptions = quickUseOptions;
        }

        public async Task Setup()
        {
            await _setup.SetupQuickUseControl(_quickUseOptions.ToList());
//            _imageQueueCoordinator.UpdateImageQueuePeriodically();
//            _wallpaperCoordinator.SubscribeToWallpaperUpdates();
            _logger.Information("Application started");
        }
    }
}