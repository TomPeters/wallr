using System;
using System.Collections.Generic;
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
        private readonly WallrHub _wallrHub;
        private List<IDisposable> _eventSubscriptions;

        public ClientEventSender(IImageStreamUpdateEvents imageStreamUpdateEvents, WallrHub wallrHub)
        {
            _imageStreamUpdateEvents = imageStreamUpdateEvents;
            _wallrHub = wallrHub;
        }

        public void StartSendingEvents()
        {
            _eventSubscriptions = new List<IDisposable>();
            _eventSubscriptions.Add(_imageStreamUpdateEvents.ImageStreamUpdateRequested.Subscribe(i =>
                _wallrHub.SendEvent("ImageStreamUpdate", new
                    {
                        NumberOfImagesRequested = i
                    })
                ));
        }

        public void Dispose()
        {
            _eventSubscriptions.ForEach(s => s.Dispose());
        }
    }
}