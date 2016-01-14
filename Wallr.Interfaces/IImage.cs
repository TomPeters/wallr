using System.IO;

namespace Wallr.Interfaces
{
    public interface IImage
    {
        ImageId ImageId { get; } // TODO: should be LocalImageId for sources and ImageId for platforms, merge with source id later
        Stream FileStream { get; } // TODO: Change to byte array
    }
}