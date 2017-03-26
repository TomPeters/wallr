using System;
using Wallr.ImageSource;

namespace Wallr.ImagePersistence
{
    public interface IImageSaver
    {
        IObservable<ISavedImage> SaveImages(IImageSource imageSource, IObservable<DateTime> saveRequests);
    }
}