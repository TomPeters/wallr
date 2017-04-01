using System.IO;
using Nancy;
using Nancy.Responses;
using Optional;
using Optional.Linq;
using Wallr.Interfaces;
using Wallr.Platform;

namespace Wallr.UI.NancyModules
{
    public class ImageModule : NancyModule
    {
        public ImageModule(IPlatform platform) : base("/image")
        {
            Get["/{imageSourceId}/{localImageId}", true] = async (parameters, ct) =>
            {
                var imageSourceId = new ImageSourceId(parameters.imageSourceId);
                var localImageId = new LocalImageId(parameters.localImageId);
                var imageId = new ImageId(localImageId, imageSourceId);
                Option<Stream> imageStream = await platform.LoadImage(imageId);
                return imageStream
                    .Select<Stream, Response>(s => new StreamResponse(() => s, "image/jpeg"))
                    .ValueOr(() => new NotFoundResponse());
            };
        }
    }
}