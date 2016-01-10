using System.Collections.Generic;
using System.Linq;
using Wallr.ImageSource;

namespace Wallr.Core
{
    public interface IImageStream
    {
        IReadOnlyList<IImage> Images { get; }
    }
}