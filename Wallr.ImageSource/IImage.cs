using System.IO;
using System.Threading.Tasks;

namespace Wallr.ImageSource
{
    public interface IImage
    {
        Task<ImageId> GetId { get; }
        Task<Stream> GetImageStream();
    }
}