using System.Runtime.Remoting.Messaging;

namespace Wallr.ImageSource
{
    public interface IImageSourceFactory
    {
        ImageSourceConfiguration CreateImageSource(ImageSourceType sourceType);
    }

    public class ImageSourceFactory : IImageSourceFactory
    {
        public ImageSourceConfiguration CreateImageSource(ImageSourceType sourceType)
        {
            return null; // nocommit implement, just create subreddit source all the time for now
        }
    }
}