using System;

namespace Wallr.Core
{
    public interface IImageStreamCoordinator
    {
        void Start();
    }

    public class ImageStreamCoordinator : IImageStreamCoordinator
    {
        private readonly IImageStreamPopulator _imageStreamPopulator;
        private readonly IImageStreamUpdateEvents _imageStreamUpdateEvents;
        private readonly IImageStream _imageStream;

        public ImageStreamCoordinator(IImageStreamPopulator imageStreamPopulator, IImageStreamUpdateEvents imageStreamUpdateEvents, IImageStream imageStream)
        {
            _imageStreamPopulator = imageStreamPopulator;
            _imageStreamUpdateEvents = imageStreamUpdateEvents;
            _imageStream = imageStream;
        }

        public void Start()
        {
            _imageStreamUpdateEvents.ImageStreamUpdateRequested
                .Subscribe(numItems => _imageStreamPopulator.AddImagesToStream(_imageStream, numItems));
        }
    }
}