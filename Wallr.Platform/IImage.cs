using System.IO;
using Wallr.ImagePersistence;

namespace Wallr.Platform
{
    public interface IImage
    {
        ImageId ImageId { get; }
        Stream FileStream { get; }
    }
}