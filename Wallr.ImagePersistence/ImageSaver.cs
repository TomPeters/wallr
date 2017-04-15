using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;
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
            IObservable<IImage> selectMany = saveRequests
                .Select(_ => imageSource.GetLatestImages())
                .SelectMany(e => e.ToObservable()); // nocommit, move this out to IImageSource, simply pass in this observable here instead
            IConnectableObservable<ISavedImage> saves = selectMany
                .Select(i => Observable.FromAsync(() => SaveImage(i, imageSource)))
                .Concat()
                .Publish();

            _saveSubscriptions.Add(saves.Connect());
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
}