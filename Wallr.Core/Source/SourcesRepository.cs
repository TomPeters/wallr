using System.Collections.Generic;
using Wallr.Platform;

namespace Wallr.Core.Source
{
    public interface IReadOnlySourcesRepository
    {
        IReadOnlyList<IImageSourceConfiguration> SourceConfigurations { get; }
    }

    public class SourcesRepository
    {
        public SourcesRepository(IPlatform platform, ISourceSerializer sourceSerializer)
        {
            _platform = platform;
            _sourceSerializer = sourceSerializer;
        }
    }
}