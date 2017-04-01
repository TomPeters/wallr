using System;
using System.Collections.Async;

namespace Wallr.ImageSource
{
    public interface IImageSource // nocommit, implement
    {
        ImageSourceId ImageSourceId { get; }
        ImageSourceName ImageSourceName { get; }
        ImageSourceType SourceType { get; }
        IAsyncEnumerable<IImage> GetLatestImages();
        IImageSourceSettings Settings { get; }
        TimeSpan UpdateInterval { get; }
    }

    public interface IImageSourceSettings
    {
        // nocommit: visitor? for different settings types
        // nocommit, some way of updating the settings??
    }
}