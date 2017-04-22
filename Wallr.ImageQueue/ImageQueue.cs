using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Optional;
using Optional.Linq;
using Serilog;
using Wallr.ImagePersistence;
using Wallr.ImageQueue.Persistence;
using Wallr.Platform;

namespace Wallr.ImageQueue
{
    public interface IImageQueue
    {
        void StartQueuingSavedImages(IObservable<ISavedImage> savedImages);
        Task Enqueue(ISavedImage savedImage);
        Task Rehydrade(Func<IEnumerable<SourceQualifiedImageId>, IEnumerable<ISavedImage>> fetchSavedImages);
        Task<Option<ISavedImage>> Dequeue();
        IEnumerable<SourceQualifiedImageId> QueuedImageIds { get; }
    }

    public class ImageQueue : IImageQueue, IDisposable
    {
        private const string SettingsKey = "ImageQueue";
        private readonly IPersistence _persistence;
        private readonly ISourceQualifiedImageIdConverter _sourceQualifiedImageIdConverter;
        private readonly ILogger _logger;
        private Queue<ISavedImage> _queue = new Queue<ISavedImage>();
        private readonly List<IDisposable> _savedImagesSubscriptions = new List<IDisposable>();

        public ImageQueue(IPersistence persistence, ISourceQualifiedImageIdConverter sourceQualifiedImageIdConverter,
            ILogger logger)
        {
            _persistence = persistence;
            _sourceQualifiedImageIdConverter = sourceQualifiedImageIdConverter;
            _logger = logger.ForContext<ImageQueue>();
        }

        public void StartQueuingSavedImages(IObservable<ISavedImage> savedImages)
        {
            _savedImagesSubscriptions.Add(savedImages.SelectMany(i => Observable.FromAsync(t => Enqueue(i))).Subscribe());
        }

        public Task Enqueue(ISavedImage savedImage)
        {
            _logger.Information("Enqueuing image {ImageId} from source {SourceId}", savedImage.Id.ImageId.Value, savedImage.Id.SourceId.Value);
            _queue.Enqueue(savedImage);
            return PersistLatestQueueState();
        }

        public async Task Rehydrade(Func<IEnumerable<SourceQualifiedImageId>, IEnumerable<ISavedImage>> fetchSavedImages)
        {
            Option<string> persistedQueue = await _persistence.LoadSettings(SettingsKey);
            _queue = persistedQueue
                .Select(JsonConvert.DeserializeObject<IEnumerable<SSourceQualifiedImageId>>)
                .Select(ids => ids.Select(_sourceQualifiedImageIdConverter.FromSerializationModel))
                .Select(fetchSavedImages)
                .Select(i => new Queue<ISavedImage>(i))
                .ValueOr(new Queue<ISavedImage>());
        }

        public async Task<Option<ISavedImage>> Dequeue()
        {
            if (!_queue.Any()) return Option.None<ISavedImage>();
            ISavedImage image = _queue.Dequeue();
            await PersistLatestQueueState();
            _logger.Information("Dequeuing image {ImageId} from source {SourceId}", image.Id.ImageId.Value, image.Id.SourceId.Value);
            return image.Some();
        }

        public IEnumerable<SourceQualifiedImageId> QueuedImageIds => _queue.Select(i => i.Id);

        private async Task PersistLatestQueueState()
        {
            IEnumerable<SourceQualifiedImageId> sourceQualifiedImageIds = _queue.Select(i => i.Id).ToList();
            _logger.Information("Persisting queue state: {@QueuedImageIds}", sourceQualifiedImageIds);
            IEnumerable<SSourceQualifiedImageId> sQueue = sourceQualifiedImageIds.Select(_sourceQualifiedImageIdConverter.ToSerializationModel);
            await _persistence.SaveSettings(SettingsKey, JsonConvert.SerializeObject(sQueue));
            _logger.Information("Queue state persisted");
        }

        public void Dispose()
        {
            foreach(IDisposable subscription in _savedImagesSubscriptions)
                subscription.Dispose();
        }
    }
}