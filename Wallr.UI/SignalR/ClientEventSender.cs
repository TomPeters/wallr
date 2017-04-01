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
        private readonly IHubContext _hubContext;
        private List<IDisposable> _eventSubscriptions;

        public ClientEventSender(IHubContext hubContext)
        {
            _hubContext = hubContext;
        }

        public void StartSendingEvents()
        {
            _eventSubscriptions = new List<IDisposable>();
//            _eventSubscriptions.Add(_imageQueueUpdateEvents.ImageQueueUpdateRequested.Subscribe(i =>
//            SendEvent("ImageQueueUpdate", new
//                {
//                    NumberOfImagesRequested = i
//                })
//            ));
//            _eventSubscriptions.Add(_wallpaperUpdateEvents.UpdateImageRequested.Subscribe(i =>
//                SendEvent("WallpaperUpdate", new
//                {
//                    TimeSinceLastUpdate = i
//                })
//            ));
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