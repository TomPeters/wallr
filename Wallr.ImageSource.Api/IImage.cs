using System.IO;
using System.Threading.Tasks;

namespace Wallr.ImageSource.Api
{
    public interface IImage
    {
        Task<string> Id { get; }
        Task<Stream> GetImageStream();
    }
}