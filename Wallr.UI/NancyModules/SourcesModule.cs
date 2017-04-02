using System.Linq;
using Nancy;
using Nancy.ModelBinding;
using Wallr.ImageSource;
using Wallr.UI.Models;

namespace Wallr.UI.NancyModules
{
    public class SourcesModule : NancyModule
    {
        public SourcesModule(IImageSources sources, IImageSourceFactory sourceFactory) : base("/sources")
        {
            Get["/"] = _ => sources.Select(s => new ImageSourceViewModel(s.ImageSourceId.Value, s.SourceType.Value));
            Post["/add", true] = async (parameters, ctx) =>
            {
                var request = this.Bind<AddRequestModel>();
                IImageSource source = sourceFactory.CreateImageSource(new ImageSourceType(request.SourceType));
                await sources.Add(source);
                return source.ImageSourceId;
            };
        }
    }

    public class AddRequestModel
    {
        public string SourceType { get; set; }
    }
}