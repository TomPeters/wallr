using Nancy;

namespace Wallr.UI.NancyModules
{
    public class ViewsModule : NancyModule
    {
        public ViewsModule()
        {
            Get["/"] = _ => Response.AsFile("Content/index.html", "text/html");
        }
    }
}