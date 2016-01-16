﻿using System;
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
            try
            {
                _imageSource.Images
                    .Select(i => new Image(i, _imageSource.ImageSourceId))
                    .Where(i => !imageSourceIds.Contains(i.ImageId.LocalImageId))
                    .Take(numberOfNewImagesToAdd)
                    .ForEach(imageStream.PushImage);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "An error ocurred when populating the stream");
            }
        }
    }

    public class Image : IImage // nocommit new file
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