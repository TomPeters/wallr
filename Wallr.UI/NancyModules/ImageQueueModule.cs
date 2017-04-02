using System.Linq;
using Nancy;
using Wallr.ImageQueue;
using Wallr.UI.Models;

namespace Wallr.UI.NancyModules
{
    public class ImageQueueModule : NancyModule
    {
        public ImageQueueModule(IImageQueue imageQueue) : base("/imageQueue")
        {
            Get["/images"] = _ => imageQueue.QueuedImageIds.Select(id => new ImageIdViewModel(id));
        }
    }
}