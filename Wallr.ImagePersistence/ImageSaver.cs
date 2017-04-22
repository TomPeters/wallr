using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using Serilog;
using Wallr.ImageSource;

namespace Wallr.ImagePersistence
{
    public interface IImageSaver
    {
        IObservable<ISavedImage> StartSavingImages(IImageSources imageSources);
    }

    public class ImageSaver : IImageSaver, IDisposable
    {
        private readonly IImageRepository _imageRepository;
        private readonly ILogger _logger;
        private readonly List<IDisposable> _saveSubscriptions = new List<IDisposable>();

        public ImageSaver(IImageRepository imageRepository, ILogger logger)
        {
            _imageRepository = imageRepository;
            _logger = logger.ForContext<ImageSaver>();
        }

        public IObservable<ISavedImage> StartSavingImages(IImageSources imageSources)
        {
            IConnectableObservable<ISavedImage> saves = imageSources.ImagesFromAllSources
                .Select(i => Observable.FromAsync(() => SaveImage(i.Image, i.SourceId)))
                .Concat()
                .Publish();

            _saveSubscriptions.Add(saves.Connect());
            return saves;
        }

        private async Task<ISavedImage> SaveImage(IImage image, ImageSourceId sourceId)
        {
            ImageId id = await image.GetId();
            _logger.Information("Saving image {ImageId} from source {SourceId}", id.Value, sourceId.Value);
            ISavedImage savedImage = await _imageRepository.SaveImage(new SourceQualifiedImageId(sourceId, id), image.GetImageStream);
            _logger.Information("Saved image {ImageId} from source {SourceId}", id.Value, sourceId.Value);
            return savedImage;
        }

        public void Dispose()
        {
            foreach(IDisposable saveSubscription in _saveSubscriptions) saveSubscription.Dispose();
        }
    }
}