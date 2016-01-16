using System;

namespace Wallr.UI
{
    public interface IWallrUiServer
    {
        void StartServer();
    }

    public class WallrUiServer : IDisposable // TODO: Implement server
    {
        public void Dispose()
        {
        }
    }
}