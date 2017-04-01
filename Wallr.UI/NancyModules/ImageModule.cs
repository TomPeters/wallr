using System.IO;
using Nancy;
using Nancy.Responses;
using Optional;
using Optional.Linq;
using Wallr.ImagePersistence;
using Wallr.ImageSource;
using ImageId = Wallr.ImageSource.ImageId;

namespace Wallr.UI.NancyModules
{
    public class ImageModule : NancyModule
    {
        public ImageModule(IImagePersistence imagePersistence) : base("/image")
        {
            Get["/{imageSourceId}/{localImageId}", true] = async (parameters, ct) =>
            {
                var imageSourceId = new ImageId(parameters.imageSourceId);
                var localImageId = new ImageSourceId(parameters.localImageId);
                var imageId = new SourceQualifiedImageId(localImageId, imageSourceId);
                Option<Stream> imageStream = await imagePersistence.LoadImage(imageId);
                return imageStream
                    .Select<Stream, Response>(s => new StreamResponse(() => s, "image/jpeg"))
                    .ValueOr(() => new NotFoundResponse());
            };
        }
    }
}