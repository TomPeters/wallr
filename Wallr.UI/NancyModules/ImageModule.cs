using System.IO;
using Nancy;
using Nancy.Responses;
using Optional;
using Optional.Linq;
using Wallr.ImageSource;
using Wallr.Platform;
using ImageId = Wallr.ImagePersistence.ImageId;

namespace Wallr.UI.NancyModules
{
    public class ImageModule : NancyModule
    {
        public ImageModule(IPlatform platform) : base("/image")
        {
            Get["/{imageSourceId}/{localImageId}", true] = async (parameters, ct) =>
            {
                var imageSourceId = new ImageSource.ImageId(parameters.imageSourceId);
                var localImageId = new ImageSourceId(parameters.localImageId);
                var imageId = new ImageId(localImageId, imageSourceId);
                Option<Stream> imageStream = await platform.LoadImage(imageId);
                return imageStream
                    .Select<Stream, Response>(s => new StreamResponse(() => s, "image/jpeg"))
                    .ValueOr(() => new NotFoundResponse());
            };
        }
    }
}