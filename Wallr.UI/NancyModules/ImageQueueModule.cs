using System.Linq;
using System.Runtime.Remoting.Messaging;
using Nancy;
using Wallr.UI.Models;

namespace Wallr.UI.NancyModules
{
    public class ImageQueueModule : NancyModule
    {
        // nocommit
        public ImageQueueModule(/*IImageQueue imageQueue*/) : base("/imageQueue")
        {
//            Get["/images"] = _ => imageQueue.ImageIds.Select(id => new ImageIdViewModel(id));
            Get["/images"] = _ => Enumerable.Empty<ImageIdViewModel>();
        }
    }
}