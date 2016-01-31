using Microsoft.Owin.Cors;
using Owin;

namespace Wallr.UI.SignalR
{
    public class SignalRStartup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(CorsOptions.AllowAll);
            app.MapSignalR();
        }
    }
}