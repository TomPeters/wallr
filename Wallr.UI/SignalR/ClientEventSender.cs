using System;
using System.Collections.Generic;
using Microsoft.AspNet.SignalR;
using Wallr.ImageQueue;

namespace Wallr.UI.SignalR
{
    public interface IClientEventSender
    {
        void StartSendingEvents();
    }

    public class ClientEventSender : IDisposable, IClientEventSender
    {
        private readonly IHubContext _hubContext;
        private readonly IImageQueueEvents _imageQueueEvents;
        private List<IDisposable> _eventSubscriptions;

        public ClientEventSender(IHubContext hubContext, IImageQueueEvents imageQueueEvents)
        {
            _hubContext = hubContext;
            _imageQueueEvents = imageQueueEvents;
        }

        public void StartSendingEvents()
        {
            _eventSubscriptions?.ForEach(s => s.Dispose());
            _eventSubscriptions = new List<IDisposable>();
            _imageQueueEvents.ImageQueueChanges.Subscribe(e => SendEvent("QueueChanged", null));
        }

        private void SendEvent(string eventName, object eventArgs)
        {
            _hubContext.Clients.All.sendEvent(eventName, eventArgs);
        }

        public void Dispose()
        {
            _eventSubscriptions?.ForEach(s => s.Dispose());
        }
    }
}