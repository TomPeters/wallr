using System;
using System.IO;
using System.Threading.Tasks;
using Optional;
using Serilog;

namespace Wallr.Platform
{
    public interface IImagePersistence
    {
        Task SaveImage(Guid sourceId, string imageId, Func<Task<Stream>> createImageStream, ILogger logger);
        Task<Option<Stream>> LoadImage(Guid sourceId, string imageId);
    }
}