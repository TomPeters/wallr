using System;
using Nancy;

namespace Wallr.UI.Middleware
{
    public interface IOnErrorMiddleware
    {
        dynamic Invoke(NancyContext context, Exception exception);
    }
}