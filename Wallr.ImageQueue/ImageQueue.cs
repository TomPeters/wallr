﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
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
    public class ImageQueueChangedEvent
    {
    }

    public interface IImageQueueEvents
    {
        IObservable<ImageQueueChangedEvent> ImageQueueChanges { get; }
    }

    public interface IObservableImageQueue : IImageQueue
    {
        void StartQueuingSavedImages(IObservable<ISavedImage> savedImages);
    }

    public interface IImageQueue
    {
        Task Enqueue(ISavedImage savedImage);
        Task Rehydrade(Func<IEnumerable<SourceQualifiedImageId>, IEnumerable<ISavedImage>> fetchSavedImages);
        Task<Option<ISavedImage>> Dequeue();
        IEnumerable<SourceQualifiedImageId> QueuedImageIds { get; }
    }

    public class ImageQueue : IImageQueue
    {
        private const string SettingsKey = "ImageQueue";
        private readonly IPersistence _persistence;
        private readonly ISourceQualifiedImageIdConverter _sourceQualifiedImageIdConverter;
        private readonly ILogger _logger;
        private Queue<ISavedImage> _queue = new Queue<ISavedImage>();

        public ImageQueue(IPersistence persistence, ISourceQualifiedImageIdConverter sourceQualifiedImageIdConverter,
            ILogger logger)
        {
            _persistence = persistence;
            _sourceQualifiedImageIdConverter = sourceQualifiedImageIdConverter;
            _logger = logger.ForContext<ImageQueue>();
        }

        public async Task Enqueue(ISavedImage savedImage)
        {
            _logger.Information("Enqueuing image {ImageId} from source {SourceId}", savedImage.Id.ImageId.Value, savedImage.Id.SourceId.Value);
            _queue.Enqueue(savedImage);
            await PersistLatestQueueState();
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
    }

    public class ObservableImageQueue : IObservableImageQueue, IImageQueueEvents, IDisposable
    {
        private readonly IImageQueue _imageQueue;
        private readonly List<IDisposable> _savedImagesSubscriptions = new List<IDisposable>();
        private readonly Subject<ImageQueueChangedEvent> _queueChanges = new Subject<ImageQueueChangedEvent>();

        public ObservableImageQueue(IImageQueue imageQueue)
        {
            _imageQueue = imageQueue;
        }

        public void StartQueuingSavedImages(IObservable<ISavedImage> savedImages)
        {
            _savedImagesSubscriptions.Add(savedImages.SelectMany(i => Observable.FromAsync(t => Enqueue(i))).Subscribe());
        }

        public async Task Enqueue(ISavedImage savedImage)
        {
            await _imageQueue.Enqueue(savedImage);
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