using System.Runtime.Remoting.Messaging;

namespace Wallr.ImageSource
{
    public interface IImageSourceConfigurationFactory
    {
        ImageSourceConfiguration CreateImageSourceConfiguration(ImageSourceType sourceType);
    }

    public class ImageSourceConfigurationFactory : IImageSourceConfigurationFactory
    {
        public ImageSourceConfiguration CreateImageSourceConfiguration(ImageSourceType sourceType)
        {
            return null; // nocommit implement, just create subreddit source all the time for now
        }
    }
}