using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using Serilog;
using Wallr.Interfaces;
using Wallr.Platform;

namespace Wallr.Core
{
    public interface IImageQueue
    {
        // Should be able to have duplicates in the queue, so need another unique id here
        int Capacity { get; }
        IReadOnlyList<ImageId> ImageIds { get; }
        void PushImage(IImage image);
        ImageId PopNextImageId { get; }
    }

    // Think about threading issues here
    public class ImageQueue : IImageQueue
    {
        private readonly IPlatform _platform;
        private readonly ILogger _logger;

        public ImageQueue(IPlatform platform, ILogger logger)
        {
            var s = new BehaviorSubject<IReadOnlyList<ImageId>>(new List<ImageId>());
            _platform = platform;
            _logger = logger;
            ImageIds = new List<ImageId>();
        }

        public int Capacity => 5;
        public IReadOnlyList<ImageId> ImageIds { get; private set; }
        public void PushImage(IImage image)
        {
            _logger.Information("Adding {ImageId} to queue. Images queue: {@ImageQueue}", image.ImageId, ImageIds.Select(i => i.LocalImageId.Value));
            try
            {
                _platform.SaveWallpaper(image, _logger);
                ImageIds = ImageIds.Concat(new[] {image.ImageId}).ToList();
                _logger.Information("Image {ImageId} added to queue. Images queue: {@ImageQueue}", image.ImageId,
                    ImageIds.Select(i => i.LocalImageId.Value));
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error occurred adding image {ImageId} to queue", image.ImageId);
            }
        }

        public ImageId PopNextImageId
        {
            get
            {
                ImageId imageId = ImageIds.First();
                ImageIds = ImageIds.Skip(1).ToList();
                _logger.Information("Removing {ImageId} to queue. Images queue: {@ImageQueue}", imageId, ImageIds.Select(i => i.LocalImageId.Value));
                return imageId;
            }
        }
    }
}