using System;
using System.Collections.Generic;
using System.Linq;
using Serilog;
using Wallr.Common;
using Wallr.ImageSource;

namespace Wallr.Core
{
    public interface IImageStreamPopulator
    {
        void AddImagesToStream(IImageStream imageStream, int imagesToAdd);
    }

    public class ImageStreamPopulator : IImageStreamPopulator
    {
        private readonly IImageSource _imageSource;
        private readonly ILogger _logger;

        public ImageStreamPopulator(IImageSource imageSource, ILogger logger)
        {
            _imageSource = imageSource;
            _logger = logger;
        }

        public void AddImagesToStream(IImageStream imageStream, int imagesToAdd)
        {
            IReadOnlyList<LocalImageId> imageSourceIds = imageStream.ImageIds.Where(i => i.ImageSourceId.Equals(_imageSource.ImageSourceId))
                .Select(i => i.LocalImageId).ToList();
            int availableCapacity = imageStream.Capacity - imageStream.ImageIds.Count;
            int numberOfNewImagesToAdd = Math.Min(availableCapacity, imagesToAdd);
            _logger.Information("Adding {NumberOfNewImagesToAddFromSource} images to stream", numberOfNewImagesToAdd);
            _imageSource.Images
                .Where(i => !imageSourceIds.Contains(i.ImageId.LocalImageId))
                .Take(numberOfNewImagesToAdd)
                .ForEach(imageStream.PushImage);
        }
    }
}