using System;
using System.Collections.Generic;
using System.Linq;
using Wallr.Common;
using Wallr.ImageSource;

namespace Wallr.Core.Source
{
    public interface IImageSourceProvider
    {
        IEnumerable<IImageSource> AvailableSourceTypes { get; }
        IImageSource GetSource(ImageSourceType sourceType);
    }

    public class ImageSourceProvider : IImageSourceProvider
    {
        private readonly Lazy<IEnumerable<IImageSource>> _availableSourceTypes = new Lazy<IEnumerable<IImageSource>>(FindAvailableSourceTypes);

        private static IEnumerable<IImageSource> FindAvailableSourceTypes()
        {
            return AppDomain.CurrentDomain.GetAssemblies() // TODO: #3 This should load assemblies in the directory, rather than assume they are already in the AppDomain
                .SelectMany(a => a.GetTypes())
                .Where(t => typeof (IImageSource).IsAssignableFrom(t))
                .Where(t => t != typeof (IImageSource))
                .Select(Activator.CreateInstance).ToList().Cast<IImageSource>();
        }

        public IEnumerable<IImageSource> AvailableSourceTypes => _availableSourceTypes.Value;

        public IImageSource GetSource(ImageSourceType sourceType) => AvailableSourceTypes.First(s => s.SourceType.Equals(sourceType));
    }
}