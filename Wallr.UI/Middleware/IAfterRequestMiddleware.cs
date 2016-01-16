using Nancy;

namespace Wallr.UI.Middleware
{
    public interface IAfterRequestMiddleware
    {
        void Invoke(NancyContext context);
    }
}