using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wallr.ImagePersistence;

namespace Wallr.ImageQueue
{
    public interface IPersistingImageQueue : IImageQueue
    {
        Task Rehydrade(Func<SourceQualifiedImageId, ISavedImage> fetchSavedImages);
    }
}