using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using Optional;
using Wallr.ImagePersistence;

namespace Wallr.ImageQueue
{
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