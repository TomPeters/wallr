using System;

namespace Wallr.ImageSource.Persistence
{
    public class SImageSource
    {
        public SImageSource(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}