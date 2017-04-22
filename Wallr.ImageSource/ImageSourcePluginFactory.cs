using System.Collections.Async;
using Wallr.ImageSource.Api;

namespace Wallr.ImageSource
{
    public interface IImageSourcePluginFactory
    {
        IImageSourcePluginAdapter CreateImageSourcePlugin(ImageSourceType sourceType);
    }

    public class ImageSourcePluginFactory : IImageSourcePluginFactory
    {
        public IImageSourcePluginAdapter CreateImageSourcePlugin(ImageSourceType sourceType)
        {
            return null; // nocommit, implement
        }
    }

    public interface IImageSourcePluginAdapter
    {
        string SourceTypeName { get; }
        IAsyncEnumerable<IImage> GetImages(IImageSourceSettings settings);
    }
}