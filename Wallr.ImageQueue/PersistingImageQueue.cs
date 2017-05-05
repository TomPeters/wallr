using System;
using System.Collections.Generic;
using System.Linq;
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
    public class PersistingImageQueue : IPersistingImageQueue
    {
        private const string SettingsKey = "ImageQueue";
        private readonly IPersistence _persistence;
        private readonly IImageQueueConverter _imageQueueConverter;
        private readonly IImageQueue _queue;
        private readonly ILogger _logger;

        public PersistingImageQueue(IPersistence persistence,
            IImageQueueConverter imageQueueConverter,
            IImageQueue queue,
            ILogger logger)
        {
            _persistence = persistence;
            _imageQueueConverter = imageQueueConverter;
            _queue = queue;
            _logger = logger.ForContext<PersistingImageQueue>();
        }

        public async Task Rehydrade(Func<SourceQualifiedImageId, ISavedImage> fetchSavedImages)
        {
            _logger.Information("Attempting to load queue");
            Option<string> persistedQueue = await _persistence.LoadSettings(SettingsKey);
            await persistedQueue.Match(async s =>
            {
                _logger.Information("Loaded queue json");
                IEnumerable<SourceQualifiedImageId> deserializedQueue = _imageQueueConverter.Deserialize(s);
                _logger.Information("Queue deserialized");
                await _queue.Clear();
                await _queue.Enqueue(deserializedQueue.Select(fetchSavedImages));
            }, () =>
            {
                _logger.Information("Unable to load queue");
                return TaskConstants.Completed;
            });
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
            await _persistence.SaveSettings(SettingsKey, _imageQueueConverter.Serialize(sourceQualifiedImageIds));
            _logger.Information("Queue state persisted");
        }

        public IEnumerable<SourceQualifiedImageId> QueuedImageIds => _queue.QueuedImageIds;
    }
}