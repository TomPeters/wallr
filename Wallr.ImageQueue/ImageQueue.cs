using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Nito.AsyncEx;
using Optional;
using Optional.Linq;
using Serilog;
using Wallr.ImagePersistence;
using Wallr.ImageQueue.Persistence;
using Wallr.Platform;

namespace Wallr.ImageQueue
{
    public class ImageQueueChangedEvent
    {
    }

    public interface IImageQueueEvents
    {
        IObservable<ImageQueueChangedEvent> ImageQueueChanges { get; }
    }

    public interface IObservableImageQueue : IPersistingImageQueue
    {
        void StartQueuingSavedImages(IObservable<ISavedImage> savedImages);
    }

    public interface IPersistingImageQueue : IImageQueue
    {
        Task Rehydrade(Func<IEnumerable<SourceQualifiedImageId>, IEnumerable<ISavedImage>> fetchSavedImages);
    }

    public interface IImageQueue
    {
        Task Enqueue(IEnumerable<ISavedImage> savedImages);
        Task<Option<ISavedImage>> Dequeue();
        Task Clear();
        IEnumerable<SourceQualifiedImageId> QueuedImageIds { get; }
    }

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

    public class PersistingImageQueue : IPersistingImageQueue
    {
        private const string SettingsKey = "ImageQueue";
        private readonly IPersistence _persistence;
        private readonly ISourceQualifiedImageIdConverter _sourceQualifiedImageIdConverter;
        private readonly IImageQueue _queue;
        private readonly ILogger _logger;

        public PersistingImageQueue(IPersistence persistence,
            ISourceQualifiedImageIdConverter sourceQualifiedImageIdConverter,
            IImageQueue queue,
            ILogger logger)
        {
            _persistence = persistence;
            _sourceQualifiedImageIdConverter = sourceQualifiedImageIdConverter;
            _queue = queue;
            _logger = logger.ForContext<PersistingImageQueue>();
        }

        public async Task Rehydrade(Func<IEnumerable<SourceQualifiedImageId>, IEnumerable<ISavedImage>> fetchSavedImages)
        {
            _logger.Information("Attempting to load queue");
            Option<string> persistedQueue = await _persistence.LoadSettings(SettingsKey);
            persistedQueue.MatchNone(() => _logger.Information("Unable to load queue, image queue will be empty"));
            persistedQueue.MatchSome(_ => _logger.Information("Loaded queue json"));
            IEnumerable<ISavedImage> savedImages = persistedQueue
                .Select(JsonConvert.DeserializeObject<IEnumerable<SSourceQualifiedImageId>>)
                .Select(ids => ids.Select(_sourceQualifiedImageIdConverter.FromSerializationModel))
                .Select(fetchSavedImages)
                .ValueOr(Enumerable.Empty<ISavedImage>());
            persistedQueue.MatchSome(_ => _logger.Information("Queue deserialized"));
            await _queue.Clear();
            await _queue.Enqueue(savedImages);
        }

        public async Task Enqueue(IEnumerable<ISavedImage> savedImages)
        {
            await _queue.Enqueue(savedImages);
            await PersistLatestQueueState();
        }

        public async Task<Option<ISavedImage>> Dequeue()
        {
            Option<ISavedImage> dequeuedImage = await _queue.Dequeue();
            await dequeuedImage.Select(_ => PersistLatestQueueState()).ValueOr(Task.FromResult(0));
            return dequeuedImage;
        }

        public async Task Clear()
        {
            await _queue.Clear();
            await PersistLatestQueueState();
        }

        private async Task PersistLatestQueueState()
        {
            IEnumerable<SourceQualifiedImageId> sourceQualifiedImageIds = _queue.QueuedImageIds.ToList();
            _logger.Information("Persisting queue state: {@QueuedImageIds}", sourceQualifiedImageIds);
            IEnumerable<SSourceQualifiedImageId> sQueue = sourceQualifiedImageIds.Select(_sourceQualifiedImageIdConverter.ToSerializationModel);
            await _persistence.SaveSettings(SettingsKey, JsonConvert.SerializeObject(sQueue));
            _logger.Information("Queue state persisted");
        }

        public IEnumerable<SourceQualifiedImageId> QueuedImageIds => _queue.QueuedImageIds;
    }

    public class ObservableImageQueue : IObservableImageQueue, IImageQueueEvents, IDisposable
    {
        private readonly IPersistingImageQueue _imageQueue;
        private readonly List<IDisposable> _savedImagesSubscriptions = new List<IDisposable>();
        private readonly Subject<ImageQueueChangedEvent> _queueChanges = new Subject<ImageQueueChangedEvent>();

        public ObservableImageQueue(IPersistingImageQueue imageQueue)
        {
            _imageQueue = imageQueue;
        }

        public void StartQueuingSavedImages(IObservable<ISavedImage> savedImages)
        {
            _savedImagesSubscriptions.Add(savedImages.SelectMany(i => Observable.FromAsync(t => Enqueue(new [] {i}))).Subscribe());
        }

        public async Task Enqueue(IEnumerable<ISavedImage> savedImages)
        {
            await _imageQueue.Enqueue(savedImages);
            _queueChanges.OnNext(new ImageQueueChangedEvent());
        }

        public async Task Rehydrade(Func<IEnumerable<SourceQualifiedImageId>, IEnumerable<ISavedImage>> fetchSavedImages)
        {
            await _imageQueue.Rehydrade(fetchSavedImages);
            _queueChanges.OnNext(new ImageQueueChangedEvent());
        }

        public async Task<Option<ISavedImage>> Dequeue()
        {
            Option<ISavedImage> dequeueImage = await _imageQueue.Dequeue();
            dequeueImage.MatchSome(_ => _queueChanges.OnNext(new ImageQueueChangedEvent()));
            return dequeueImage;
        }

        public async Task Clear()
        {
            await _imageQueue.Clear();
            _queueChanges.OnNext(new ImageQueueChangedEvent());
        }

        public IEnumerable<SourceQualifiedImageId> QueuedImageIds => _imageQueue.QueuedImageIds;
        public IObservable<ImageQueueChangedEvent> ImageQueueChanges => _queueChanges;

        public void Dispose()
        {
            foreach(IDisposable subscription in _savedImagesSubscriptions)
                subscription.Dispose();
            _queueChanges.Dispose();
        }
    }
}