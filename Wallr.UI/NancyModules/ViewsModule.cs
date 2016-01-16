using System.Runtime.Remoting.Messaging;
using Nancy;

namespace Wallr.UI.NancyModules
{
    public class ViewsModule : NancyModule
    {
        public ViewsModule()
        {
            Get["/"] = _ => "Hello world";
        }
    }
}