﻿using System.Linq;
using System.Runtime.Remoting.Messaging;
using Nancy;
using Wallr.Core;
using Wallr.UI.Models;

namespace Wallr.UI.NancyModules
{
    public class ImageQueueModule : NancyModule
    {
        public ImageQueueModule(IImageQueue imageQueue) : base("/imageQueue")
        {
            Get["/images"] = _ => imageQueue.ImageIds.Select(id => new ImageIdViewModel(id));
        }
    }
}