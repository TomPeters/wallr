using System.Linq;
using Nancy;
using Nancy.ModelBinding;
using Wallr.ImageSource;
using Wallr.UI.Models;

namespace Wallr.UI.NancyModules
{
    public class SourcesModule : NancyModule
    {
        public SourcesModule(IImageSourceConfigurations sourceConfigurations, IImageSourceFactory sourceFactory) : base("/sources")
        {
            Get["/"] = _ => sourceConfigurations.Select(s => new ImageSourceViewModel(s.ImageSourceId.Value, s.SourceType.Value));
            Post["/add", true] = async (parameters, ctx) =>
            {
                var request = this.Bind<AddRequestModel>();
                ImageSourceConfiguration source = sourceFactory.CreateImageSource(new ImageSourceType(request.SourceType));
                await sourceConfigurations.Add(source);
                return source.ImageSourceId;
            };
        }
    }

    public class AddRequestModel
    {
        public string SourceType { get; set; }
    }
}