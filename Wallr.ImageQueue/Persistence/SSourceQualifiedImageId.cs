using System;
using Wallr.ImagePersistence;

namespace Wallr.ImageQueue.Persistence
{
    public class SSourceQualifiedImageId
    {
        public SSourceQualifiedImageId(Guid source, string id)
        {
            Source = source;
            Id = id;
        }

        public Guid Source { get; }
        public string Id { get; }
    }
}