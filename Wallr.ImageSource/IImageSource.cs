using System.Collections.Generic;

namespace Wallr.ImageSource
{
    public interface IImageSource
    {
        IImage FetchLatestImage(); // Should ideally return a stream of images
    }

    public interface IImage
    {
        ImageId Id { get; } 
    }
}