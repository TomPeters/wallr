using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nito.AsyncEx;
using Optional;
using Serilog;
using Wallr.ImagePersistence;

namespace Wallr.ImageQueue
{
    public class ImageQueue : IImageQueue
    {
        private const string SettingsKey = "ImageQueue";
        private readonly ILogger _logger;
        private readonly Queue<ISavedImage> _queue = new Queue<ISavedImage>();

        public ImageQueue(ILogger logger)
        {
            _logger = logger.ForContext<ImageQueue>();
        }

        public Task Enqueue(IEnumerable<ISavedImage> savedImages)
        {
            foreach (ISavedImage savedImage in savedImages)
            {
                _logger.Information("Enqueuing image {ImageId} from source {SourceId}", savedImage.Id.ImageId.Value, savedImage.Id.SourceId.Value);
                _queue.Enqueue(savedImage);
            }
            return TaskConstants.Completed;
        }

        public Task<Option<ISavedImage>> Dequeue()
        {
            if (!_queue.Any()) return Task.FromResult(Option.None<ISavedImage>());
            ISavedImage image = _queue.Dequeue();
            _logger.Information("Dequeuing image {ImageId} from source {SourceId}", image.Id.ImageId.Value, image.Id.SourceId.Value);
            return Task.FromResult(image.Some());
        }

        public Task Clear()
        {
            _logger.Information("Clearing entire image queue");
            _queue.Clear();
            return TaskConstants.Completed;
        }

        public IEnumerable<SourceQualifiedImageId> QueuedImageIds => _queue.Select(i => i.Id);
    }
}