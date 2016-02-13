using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Serilog;
using Wallr.Common;
using Wallr.ImageSource;
using Wallr.Interfaces;
using Wallr.Platform;

namespace Wallr.Core
{
    public interface IImageQueuePopulator
    {
        void AddImagesToQueue(IImageQueue imageQueue, int imagesToAdd);
    }

    public class ImageQueuePopulator : IImageQueuePopulator
    {
        private readonly IImageSource _imageSource;
        private readonly ILogger _logger;

        public ImageQueuePopulator(IImageSource imageSource, ILogger logger)
        {
            _imageSource = imageSource;
            _logger = logger;
        }

        public void AddImagesToQueue(IImageQueue imageQueue, int imagesToAdd)
        {
            IReadOnlyList<LocalImageId> imageSourceIds = imageQueue.ImageIds.Where(i => i.ImageSourceId.Equals(_imageSource.ImageSourceId))
                .Select(i => i.LocalImageId).ToList();
            int availableCapacity = imageQueue.Capacity - imageQueue.ImageIds.Count;
            int numberOfNewImagesToAdd = Math.Min(availableCapacity, imagesToAdd);
            _logger.Information("Adding {NumberOfNewImagesToAddFromSource} images to queue", numberOfNewImagesToAdd);
            try
            {
                _imageSource.Images
                    .Select(i => new Image(i, _imageSource.ImageSourceId))
                    .Where(i => !imageSourceIds.Contains(i.ImageId.LocalImageId))
                    .Take(numberOfNewImagesToAdd)
                    .ForEach(imageQueue.PushImage);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "An error ocurred when populating the queue");
            }
        }
    }

    public class Image : IImage // TODO new file
    {
        private readonly ISourceImage _sourceImage;
        private readonly ImageSourceId _imageSourceId;

        public Image(ISourceImage sourceImage, ImageSourceId imageSourceId)
        {
            _sourceImage = sourceImage;
            _imageSourceId = imageSourceId;
        }

        public ImageId ImageId => new ImageId(_sourceImage.ImageId, _imageSourceId);
        public Stream FileStream => _sourceImage.FileStream;
    }
}