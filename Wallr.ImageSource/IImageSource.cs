using System.Collections.Generic;
using System.IO;

namespace Wallr.ImageSource
{
    public interface IImageSource
    {
        IImage FetchLatestImage(); // Should ideally return a stream of images
    }

    public interface IImage
    {
        ImageId Id { get; }
        string FileName { get; }
        Stream FileStream { get; }
    }
}