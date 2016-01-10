using System.IO;

namespace Wallr.ImageSource
{
    public interface IImageSource
    {
        IImage FetchLatestImage(); // Should ideally return a stream of images
    }

    public interface IImage
    {
        StreamImageId StreamImageId { get; }
        Stream FileStream { get; }
    }
}