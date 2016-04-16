using System.Collections.Generic;
using System.Linq;
using Wallr.Common;
using Wallr.ImageSource;

namespace Wallr.Core.Source
{
    public interface ISourceTypeProvider
    {
        IEnumerable<IThinImageSource> AvailableSourceTypes { get; }
        IThinImageSource GetSource(ImageSourceType sourceType);
    }

    public class SourceTypeProvider : ISourceTypeProvider
    {
        public IEnumerable<IThinImageSource> AvailableSourceTypes => Enumerable.Empty<IThinImageSource>(); // nocommit implement
        public IThinImageSource GetSource(ImageSourceType sourceType) => null; // nocommit implement
    }
}