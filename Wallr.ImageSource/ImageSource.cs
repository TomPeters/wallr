using System;
using System.Reactive.Linq;

namespace Wallr.ImageSource
{
    public interface IImageSource
    {
        ImageSourceId ImageSourceId { get; }
        IObservable<IImage> Images { get; }
    }

    public class ImageSource : IImageSource
    {
        private readonly ImageSourceConfiguration _configuration;
        private readonly IImageSourcePluginFactory _imageSourcePluginFactory;

        public ImageSource(ImageSourceConfiguration configuration, IImageSourcePluginFactory imageSourcePluginFactory)
        {
            _configuration = configuration;
            _imageSourcePluginFactory = imageSourcePluginFactory;
        }

        public ImageSourceId ImageSourceId => _configuration.ImageSourceId;

        public IObservable<IImage> Images
        {
            get
            {
                return Observable.Interval(_configuration.UpdateInterval)
                    .Select(_ => _imageSourcePluginFactory.CreateImageSourcePlugin(_configuration.SourceType)
                        .GetImages(_configuration.Settings))
                    .SelectMany(e => e.ToObservable());
            }
        }
    }
}