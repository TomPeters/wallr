using Nancy;

namespace Wallr.UI.Middleware
{
    public interface IBeforeRequestMiddleware
    {
        Response Invoke(NancyContext context);
    }
}