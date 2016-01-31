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
        private List<IDisposable> _eventSubscriptions;

        public ClientEventSender(IImageStreamUpdateEvents imageStreamUpdateEvents, IHubContext hubContext)
        {
            _imageStreamUpdateEvents = imageStreamUpdateEvents;
            _hubContext = hubContext;
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