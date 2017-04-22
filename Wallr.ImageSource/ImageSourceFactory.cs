using System.Runtime.Remoting.Messaging;

namespace Wallr.ImageSource
{
    public interface IImageSourceFactory
    {
        IImageSource CreateImageSource(IImageSourceConfiguration imageSourceConfiguration);
    }

    public class ImageSourceFactory : IImageSourceFactory
    {
        private readonly IImageSourcePluginFactory _imageSourcePluginFactory;

        public ImageSourceFactory(IImageSourcePluginFactory imageSourcePluginFactory)
        {
            _imageSourcePluginFactory = imageSourcePluginFactory;
        }

        public IImageSource CreateImageSource(IImageSourceConfiguration imageSourceConfiguration)
        {
            return new ImageSource(imageSourceConfiguration, _imageSourcePluginFactory);
        }
    }
}