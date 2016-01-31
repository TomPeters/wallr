using Microsoft.AspNet.SignalR;

namespace Wallr.UI.SignalR
{
    public class WallrHub : Hub
    {
        public void Send(string name, string message)
        {
            Clients.All.addMessage(name, message);
        }
    }
}