using System.IO;
using System.Threading.Tasks;
using Optional;

namespace Wallr.ImagePersistence
{
    public interface ISavedImage
    {
        SourceQualifiedImageId Id { get; }
        Task<Option<Stream>> GetStream();
    }
}