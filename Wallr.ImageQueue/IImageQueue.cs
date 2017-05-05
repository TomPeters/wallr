using System.Collections.Generic;
using System.Threading.Tasks;
using Optional;
using Wallr.ImagePersistence;

namespace Wallr.ImageQueue
{
    public interface IImageQueue
    {
        Task Enqueue(IEnumerable<ISavedImage> savedImages);
        Task<Option<ISavedImage>> Dequeue();
        Task Clear();
        IEnumerable<SourceQualifiedImageId> QueuedImageIds { get; }
    }
}