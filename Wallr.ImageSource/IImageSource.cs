using System.Collections.Generic;
using Wallr.Interfaces;

namespace Wallr.ImageSource
{
    public interface IImageSource
    {
        IEnumerable<ISourceImage> Images { get; }
        ImageSourceId ImageSourceId { get; }
    }
}