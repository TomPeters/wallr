using System;
using System.IO;
using System.Threading.Tasks;
using Wallr.ImagePersistence;

namespace Wallr.ImageQueue
{
    public interface IImageQueue
    {
        void ConnectSource(IObservable<ISavedImage> images); // On startup, Connect a rehydrating source that loads saved images from the persisted queue
        ISavedImage Dequeue();
    }

    public class ImageQueue : IImageQueue
    {
        public void ConnectSource(IObservable<ISavedImage> images)
        {
            throw new NotImplementedException();
        }

        public ISavedImage Dequeue()
        {
            throw new NotImplementedException();
        }
    }
}