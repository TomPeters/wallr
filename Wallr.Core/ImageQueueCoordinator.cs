using System;

namespace Wallr.Core
{
    public interface IImageQueueCoordinator
    {
        void Start();
    }

    public class ImageQueueCoordinator : IImageQueueCoordinator
    {
        private readonly IImageQueuePopulator _imageQueuePopulator;
        private readonly IImageQueueUpdateEvents _imageQueueUpdateEvents;
        private readonly IImageQueue _imageQueue;

        public ImageQueueCoordinator(IImageQueuePopulator imageQueuePopulator, IImageQueueUpdateEvents imageQueueUpdateEvents, IImageQueue imageQueue)
        {
            _imageQueuePopulator = imageQueuePopulator;
            _imageQueueUpdateEvents = imageQueueUpdateEvents;
            _imageQueue = imageQueue;
        }

        public void Start()
        {
            _imageQueueUpdateEvents.ImageQueueUpdateRequested
                .Subscribe(numItems => _imageQueuePopulator.AddImagesToQueue(_imageQueue, numItems));
        }
    }
}