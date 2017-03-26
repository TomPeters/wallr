using System;
using System.Collections.Async;
using System.IO;
using System.Threading.Tasks;

namespace Wallr.ImageSource
{
    public interface IImageSource
    {
        Guid ImageSourceId { get; } // nocommit, typed?
        string ImageSourceName { get; } // nocommit, typed?
        ImageSourceType SourceType { get; }
        IAsyncEnumerable<IImage2> GetLatestImages();
        IImageSourceSettings Settings { get; }
    }

    public interface IImageSourceSettings
    {
        // nocommit: visitor? for different settings types
    }

    public interface IImage2
    {
        string Id { get; }
        Task<Stream> GetImageStream();
    }
}