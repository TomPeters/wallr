using Nancy;
using Nancy.Responses;
using Wallr.Interfaces;
using Wallr.Platform;

namespace Wallr.UI.NancyModules
{
    public class ImageModule : NancyModule
    {
        public ImageModule(IPlatform platform) : base("/image")
        {
            Get["/{imageSourceId}/{localImageId}"] = parameters =>
            {
                var imageSourceId = new ImageSourceId(parameters.imageSourceId);
                var localImageId = new LocalImageId(parameters.localImageId);
                var imageId = new ImageId(localImageId, imageSourceId);
                return new StreamResponse(() => platform.LoadImage(imageId), "image/jpeg"); // TODO: Platform agnostic image content type?
            };
        }
    }
}