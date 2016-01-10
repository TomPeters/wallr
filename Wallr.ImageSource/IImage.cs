using System.IO;

namespace Wallr.ImageSource
{
    public interface IImage
    {
        ImageId ImageId { get; } // TODO: should be LocalImageId, merge with source id later
        Stream FileStream { get; }
    }
}