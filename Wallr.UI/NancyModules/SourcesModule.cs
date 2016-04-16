using System.Linq;
using Nancy;
using Wallr.Common;
using Wallr.Core;
using Wallr.Core.Source;
using Wallr.ImageSource;
using Wallr.UI.Models;

namespace Wallr.UI.NancyModules
{
    public class SourcesModule : NancyModule
    {
        public SourcesModule(ISourcesRepository sourcesRepository, ISourceTypeProvider sourceTypeProvider) : base("/sources")
        {
            Get["/"] = _ => sourcesRepository.SourceConfigurations.Select(s => new ImageSourceViewModel(s));
            Post["/add"] = parameters =>
            {
                var sourceType = new ImageSourceType(parameters.SourceType);
                IThinImageSource source = sourceTypeProvider.GetSource(sourceType);
                var imageSourceId = new ConfiguredImageSourceId();
                sourcesRepository.AddSourceConfiguration(new ImageSourceConfiguration(imageSourceId, sourceType, source.DefaultSettings));
                return imageSourceId.Value;
            };
        }
    }
}