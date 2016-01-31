using System;
using System.Collections.Generic;
using Microsoft.AspNet.SignalR;
using Wallr.Core;

namespace Wallr.UI.SignalR
{
    public interface IClientEventSender
    {
        void StartSendingEvents();
    }

    public class ClientEventSender : IDisposable, IClientEventSender
    {
        private readonly IImageStreamUpdateEvents _imageStreamUpdateEvents;
        private readonly IHubContext _hubContext;
        private readonly IWallpaperUpdateEvents _wallpaperUpdateEvents;
        private List<IDisposable> _eventSubscriptions;

        public ClientEventSender(IImageStreamUpdateEvents imageStreamUpdateEvents, IHubContext hubContext, IWallpaperUpdateEvents wallpaperUpdateEvents)
        {
            _imageStreamUpdateEvents = imageStreamUpdateEvents;
            _hubContext = hubContext;
            _wallpaperUpdateEvents = wallpaperUpdateEvents;
        }

        public void StartSendingEvents()
        {
            _eventSubscriptions = new List<IDisposable>();
            _eventSubscriptions.Add(_imageStreamUpdateEvents.ImageStreamUpdateRequested.Subscribe(i =>
            SendEvent("ImageStreamUpdate", new
                {
                    NumberOfImagesRequested = i
                })
            ));
            _eventSubscriptions.Add(_wallpaperUpdateEvents.UpdateImageRequested.Subscribe(i =>
                SendEvent("WallpaperUpdate", new
                {
                    TimeSinceLastUpdate = i
                })
            ));
        }

        private void SendEvent(string eventName, object eventArgs)
        {
            _hubContext.Clients.All.sendEvent(eventName, eventArgs);
        }

        public void Dispose()
        {
            _eventSubscriptions.ForEach(s => s.Dispose());
        }
    }
}