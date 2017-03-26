using System.Collections.Async;

namespace Wallr.ImageSource.Api
{
    public interface IImageSource<in TSettings> // TSettings is an interface, can use attributes to define extra properties like defaults, order and optional
    {
        string SourceTypeName { get; }
        IAsyncEnumerable<IImage> GetImages(TSettings settings);
    }
}