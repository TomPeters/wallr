using System;
using System.Collections.Generic;
using System.Linq;
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
}