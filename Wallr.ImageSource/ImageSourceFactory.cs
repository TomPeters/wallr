using System.Runtime.Remoting.Messaging;

namespace Wallr.ImageSource
{
    public interface IImageSourceFactory
    {
        IImageSource CreateImageSource(ImageSourceType sourceType);
    }

    public class ImageSourceFactory : IImageSourceFactory
    {
        public IImageSource CreateImageSource(ImageSourceType sourceType)
        {
            return null; // nocommit implement, just create subreddit source all the time for now
        }
    }
}