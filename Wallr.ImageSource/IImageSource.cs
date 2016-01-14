using System.Collections.Generic;
using Wallr.Interfaces;

namespace Wallr.ImageSource
{
    public interface IImageSource
    {
        IEnumerable<IImage> Images { get; }
        ImageSourceId ImageSourceId { get; }
    }
}