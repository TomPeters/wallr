using System.IO;

namespace Wallr.ImageSource
{
    public interface IImageSource
    {
        IImage FetchLatestImage(); // Should ideally return a stream of images
    }

    //public interface IImage
    //{
    //    IImage 
    //}

    public interface IImage
    {
        ImageId ImageId { get; }
        Stream FileStream { get; }
    }
}