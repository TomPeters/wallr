using System.Runtime.Remoting.Messaging;

namespace Wallr.ImageSource
{
    public class ImageSourceFactory
    {
        private readonly IImageSourcePluginFactory _imageSourcePluginFactory;

        public ImageSourceFactory(IImageSourcePluginFactory imageSourcePluginFactory)
        {
            _imageSourcePluginFactory = imageSourcePluginFactory;
        }

        public IImageSource CreateImageSource(ImageSourceConfiguration imageSourceConfiguration)
        {
            return new ImageSource(imageSourceConfiguration, _imageSourcePluginFactory);
        }
    }
}