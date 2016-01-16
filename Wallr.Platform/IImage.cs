using System.IO;

namespace Wallr.Platform
{
    public interface IImage
    {
        ImageId ImageId { get; }
        Stream FileStream { get; } // TODO: Change to byte array
    }
}