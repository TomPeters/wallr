using System.IO;
using Wallr.Interfaces;

namespace Wallr.ImageSource
{
    public interface ISourceImage
    {
        LocalImageId ImageId { get; }
        Stream FileStream { get; } // TODO: Change to byte array
    }
}