using System.Linq;
using Nancy;
using Nancy.ModelBinding;
using Wallr.Common;
using Wallr.Core;
using Wallr.Core.Source;
using Wallr.ImageSource;
using Wallr.UI.Models;

namespace Wallr.UI.NancyModules
{
    public class SourcesModule : NancyModule
    {
        public SourcesModule(ISourcesRepository sourcesRepository, IImageSourceProvider imageSourceProvider) : base("/sources")
        {
            Get["/"] = _ => sourcesRepository.SourceConfigurations.Select(s => new ImageSourceViewModel(s));
            Post["/add"] = _ =>
            {
                AddRequestModel request = this.Bind<AddRequestModel>();
                var sourceType = new ImageSourceType(request.SourceType);
                IImageSource source = imageSourceProvider.GetSource(sourceType);
                var imageSourceId = new ConfiguredImageSourceId();
                sourcesRepository.AddSourceConfiguration(new ImageSourceConfiguration(imageSourceId, sourceType, source.DefaultSettings));
                return imageSourceId.Value;
            };
        }
    }

    public class AddRequestModel
    {
        public string SourceType { get; set; }
    }
}