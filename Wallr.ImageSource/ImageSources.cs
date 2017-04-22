using System;
using System.Collections.Generic;
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
                return _configurations.SourceLoads.Select(load =>
                    {
                        return load.SourceConfigurations.ToObservable()
                            .Concat(_configurations.SourceAdds.Select(add => add.SourceConfiguration))
                            .SelectMany(configuration =>
                            {
                                ImageSourceId sourceId = configuration.ImageSourceId;

                                var removes = _configurations.SourceRemoves
                                    .Where(remove => remove.SourceId.Equals(sourceId));
                                var changes = _configurations.SourceChanges
                                    .Where(change => change.UpdatedSourceConfiguration.ImageSourceId.Equals(sourceId));

                                var sourceConfigurations = Observable.Return(configuration)
                                    .Concat(changes.Select(c => c.UpdatedSourceConfiguration))
                                    .TakeUntil(removes);

                                return sourceConfigurations
                                    .Select(_imageSourceFactory.CreateImageSource)
                                    .Select(s => s.Images.Select(i => new ImageFromSource(i, s.ImageSourceId)))
                                    .Switch();
                            });
                    })
                    .Switch();
            }
        }
    }
}