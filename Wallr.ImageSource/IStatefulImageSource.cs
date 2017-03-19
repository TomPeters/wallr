using System.Collections.Async;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Wallr.Interfaces;

namespace Wallr.ImageSource
{
    public interface IStatefulImageSource // TODO: Deprecated, use IImageSource instead
    {
        IEnumerable<ISourceImage> Images { get; }
        ImageSourceId ImageSourceId { get; }
    }

    public interface IImageSource<in TSettings> // TSettings is an interface, can use attributes to define extra properties like defaults, order and optional
    {
        string SourceTypeName { get; }
        IAsyncEnumerable<IImage> GetImages(TSettings settings);
    }

    public interface IImage
    {
        string Id { get; }
        Task<Stream> GetImageStream();
    }
}