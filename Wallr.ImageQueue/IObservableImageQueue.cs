using System;
using Wallr.ImagePersistence;

namespace Wallr.ImageQueue
{
    public interface IObservableImageQueue : IPersistingImageQueue
    {
        void StartQueuingSavedImages(IObservable<ISavedImage> savedImages);
    }
}