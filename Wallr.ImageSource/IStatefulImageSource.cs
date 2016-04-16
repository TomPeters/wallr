using System.Collections.Generic;
using Wallr.Common;
using Wallr.Interfaces;

namespace Wallr.ImageSource
{
    public interface IStatefulImageSource // TODO: Deprecated, move to a stateless api.
    {
        IEnumerable<ISourceImage> Images { get; }
        ImageSourceId ImageSourceId { get; }
    }

    public interface IThinImageSource
    {
        ImageSourceType SourceType { get; }
        IDictionary<string, string> DefaultSettings { get; } 
    }
}