using System.Collections.Generic;
using System.Linq;
using Serilog;
using Wallr.Interfaces;
using Wallr.Platform;

namespace Wallr.Core
{
    public interface IImageStream
    {
        // Should be able to have duplicates in the stream, so need another unique id here
        int Capacity { get; }
        IReadOnlyList<ImageId> ImageIds { get; }
        void PushImage(IImage image);
        ImageId PopNextImageId { get; }
    }

    // Think about threading issues here
    public class ImageStream : IImageStream
    {
        private readonly IPlatform _platform;
        private readonly ILogger _logger;

        public ImageStream(IPlatform platform, ILogger logger)
        {
            _platform = platform;
            _logger = logger;
            ImageIds = new List<ImageId>();
        }

        public int Capacity => 30;
        public IReadOnlyList<ImageId> ImageIds { get; private set; }
        public void PushImage(IImage image)
        {
            _platform.SaveWallpaper(image, _logger);
            ImageIds = ImageIds.Concat(new [] { image.ImageId }).ToList();
            _logger.Information("Adding {ImageId} to stream. Images stream: {@ImageStream}", image.ImageId, ImageIds.Select(i => i.LocalImageId.Value));
        }

        public ImageId PopNextImageId
        {
            get
            {
                ImageId imageId = ImageIds.First();
                ImageIds = ImageIds.Skip(1).ToList();
                _logger.Information("Removing {ImageId} to stream. Images stream: {@ImageStream}", imageId, ImageIds.Select(i => i.LocalImageId.Value));
                return imageId;
            }
        }
    }
}