using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Wallr.ImageSource
{
    public interface IImageSources : IEnumerable<IImageSource>
    {
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

        public IEnumerator<IImageSource> GetEnumerator()
        {
            return _configurations.Select(_imageSourceFactory.CreateImageSource).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}