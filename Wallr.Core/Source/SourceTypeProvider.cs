using System;
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
        private readonly Lazy<IEnumerable<IThinImageSource>> _availableSourceTypes = new Lazy<IEnumerable<IThinImageSource>>(FindAvailableSourceTypes);

        private static IEnumerable<IThinImageSource> FindAvailableSourceTypes()
        {
            return AppDomain.CurrentDomain.GetAssemblies() // TODO: #3 This should load assemblies in the directory, rather than assume they are already in the AppDomain
                .SelectMany(a => a.GetTypes())
                .Where(t => t.IsInstanceOfType(typeof (IThinImageSource)))
                .Select(Activator.CreateInstance)
                .Cast<IThinImageSource>();
        }

        public IEnumerable<IThinImageSource> AvailableSourceTypes => _availableSourceTypes.Value;

        public IThinImageSource GetSource(ImageSourceType sourceType) => AvailableSourceTypes.First(s => s.SourceType == sourceType);
    }
}