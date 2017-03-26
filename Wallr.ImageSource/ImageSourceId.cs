using System;
using Wallr.Common;

namespace Wallr.ImageSource
{
    public class ImageSourceId : TypedGuid<ImageSourceId>
    {
        public ImageSourceId(Guid value) : base(value)
        {
        }
    }
}