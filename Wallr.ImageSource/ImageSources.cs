using System;
using System.Collections;
using System.Collections.Generic;

namespace Wallr.ImageSource
{
    public interface IImageSources : IEnumerable<IImageSource>
    {
    }

    public class ImageSources : IImageSources
    {
        public ImageSources()
        {
        }

        public IEnumerator<IImageSource> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}