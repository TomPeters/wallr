using Wallr.ImagePersistence;
using Wallr.ImageQueue;
using Wallr.ImageSource;

namespace Wallr
{
    public class ImageUpdater
    {
        private readonly IImageSaver _saver;
        private readonly IImageQueue _queue;

        public ImageUpdater(IImageSaver saver, IImageQueue queue)
        {
            _saver = saver;
            _queue = queue;
        }

        public void UpdateImages(IImageSource source)
        {
            _saver.SaveImages(source.Source)
        }
    }
}