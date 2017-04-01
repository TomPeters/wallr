using System;
using System.Collections.Async;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Wallr.ImageSource;

namespace Wallr.ImagePersistence
{
    public interface IImageSaver
    {
        IObservable<ISavedImage> SaveImages(IImageSource imageSource, IObservable<DateTime> saveRequests);
    }

    public class ImageSaver : IImageSaver, IDisposable
    {
        private readonly IImageRepository _imageRepository;
        private readonly List<IDisposable> _saveSubscriptions = new List<IDisposable>();

        public ImageSaver(IImageRepository imageRepository)
        {
            _imageRepository = imageRepository;
        }

        public IObservable<ISavedImage> SaveImages(IImageSource imageSource, IObservable<DateTime> saveRequests)
        {
            var saves = saveRequests
                .Select(_ => imageSource.GetLatestImages())
                .SelectMany(e => e.ToObservable())
                .Select(i => Observable.FromAsync(() => SaveImage(i, imageSource)))
                .Concat()
                .Publish().RefCount();

            _saveSubscriptions.Add(saves.Subscribe());
            return saves;
        }

        private async Task<ISavedImage> SaveImage(IImage image, IImageSource source)
        {
            ImageSource.ImageId id = await image.GetId();
            return await _imageRepository.SaveImage(new SourceQualifiedImageId(source.ImageSourceId, id), image.GetImageStream);
        }

        public void Dispose()
        {
            foreach(IDisposable saveSubscription in _saveSubscriptions) saveSubscription.Dispose();
        }
    }

    public static class AsyncEnumerableExtensions // nocommit, new file
    {
        public static IObservable<T> ToObservable<T>(this IAsyncEnumerable<T> enumerable)
        {
            return Observable.Create<T>(obs =>
            {
                return Scheduler.Default.ScheduleAsync(async (s, t) =>
                {
                    var enumerator = await enumerable.GetAsyncEnumeratorAsync(t);
                    while (true)
                    {
                        if (t.IsCancellationRequested)
                            break;
                        try
                        {
                            bool next = await enumerator.MoveNextAsync(t);
                            if (!next)
                            {
                                obs.OnCompleted();
                                break;
                            }
                        }
                        catch (Exception ex)
                        {
                            obs.OnError(ex);
                            break;
                        }
                        obs.OnNext(enumerator.Current);
                    }
                    enumerator.Dispose();
                });
            });
        }
    }
}