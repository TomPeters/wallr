using System.Collections.Generic;

namespace Wallr.ImageSource
{
    public interface IImageSource
    {
        IEnumerable<IImage> Images { get; }
        ImageSourceId ImageSourceId { get; }
    }
}