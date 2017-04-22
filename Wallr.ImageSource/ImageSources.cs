using System;
using System.Reactive.Linq;

namespace Wallr.ImageSource
{
    public interface IImageSources
    {
        IObservable<IImageFromSource> ImagesFromAllSources { get; }
    }

    public class ImageSources : IImageSources
    {
        private readonly IImageSourceConfigurations _configurations;
        private readonly IImageSourceFactory _imageSourceFactory;

        public ImageSources(IImageSourceConfigurations configurations, IImageSourceFactory imageSourceFactory)
        {
            _configurations = configurations;
            _imageSourceFactory = imageSourceFactory;
        }

        public IObservable<IImageFromSource> ImagesFromAllSources
        {
            get
            {
                return _configurations.SourceAdds.SelectMany(newSourceEvent =>
                {
                    ImageSourceId sourceId = newSourceEvent.SourceConfiguration.ImageSourceId;

                    var removes = _configurations.SourceRemoves
                        .Where(remove => remove.SourceId.Equals(sourceId));
                    var changes = _configurations.SourceChanges
                        .Where(change => change.UpdatedSourceConfiguration.ImageSourceId.Equals(sourceId));

                    var sourceConfigurations = Observable.Return(newSourceEvent.SourceConfiguration)
                            .Concat(changes.Select(c => c.UpdatedSourceConfiguration))
                            .TakeUntil(removes);

                    return sourceConfigurations
                        .Select(_imageSourceFactory.CreateImageSource)
                        .Select(s => s.Images.Select(i => new ImageFromSource(i, s.ImageSourceId)))
                        .Switch();
                });
            }
        }
    }
}