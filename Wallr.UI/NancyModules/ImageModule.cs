using System.IO;
using Nancy;
using Nancy.Responses;
using Optional;
using Optional.Linq;
using Wallr.ImageSource;
using Wallr.Platform;
using ImageId = Wallr.ImageSource.ImageId;

namespace Wallr.UI.NancyModules
{
    public class ImageModule : NancyModule
    {
        public ImageModule(IImagePersistence imagePersistence) : base("/image")
        {
            Get["/{imageSourceId}/{localImageId}", true] = async (parameters, ct) =>
            {
                var imageSourceId = new ImageSourceId(parameters.imageSourceId);
                var localImageId = new ImageId(parameters.localImageId);
                Option<Stream> imageStream = await imagePersistence.LoadImage(imageSourceId.Value, localImageId.Value);
                return imageStream
                    .Select<Stream, Response>(s => new StreamResponse(() => s, "image/jpeg"))
                    .ValueOr(() => new NotFoundResponse());
            };
        }
    }
}