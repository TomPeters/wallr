using System;
using System.IO;
using System.Threading.Tasks;
using Optional;
using Serilog;

namespace Wallr.ImagePersistence
{
    public interface IImagePersistence
    {
        Task SaveImage(SourceQualifiedImageId sourceQualifiedImageId, Func<Task<Stream>> createImageStream, ILogger logger);
        Task<Option<Stream>> LoadImage(SourceQualifiedImageId sourceQualifiedImageId);
    }
}