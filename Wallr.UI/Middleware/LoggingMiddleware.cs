using System;
using Nancy;
using Serilog;

namespace Wallr.UI.Middleware
{
    public class LoggingMiddleware : IBeforeRequestMiddleware, IAfterRequestMiddleware, IOnErrorMiddleware
    {
        private const string StartTimeItemsKey = "StartTime";
        private readonly ILogger _logger;

        public LoggingMiddleware(ILogger logger)
        {
            _logger = logger;
        }

        Response IBeforeRequestMiddleware.Invoke(NancyContext context)
        {
            context.Items.Add(StartTimeItemsKey, DateTime.Now);
            _logger.Information("{Method} request received at {Url}", context.Request.Method, context.Request.Url);
            return null;
        }

        void IAfterRequestMiddleware.Invoke(NancyContext context)
        {
            var contextLogger = context.ResolvedRoute != null
                ? _logger.ForContext("RouteName", context.ResolvedRoute.Description.Name)
                    .ForContext("RoutePath", context.ResolvedRoute.Description.Path)
                : _logger;
            if (context.ResolvedRoute != null)
                contextLogger.Information("Request resolved to route {RoutePath}", context.ResolvedRoute.Description.Path);
            if (context.Items.ContainsKey(StartTimeItemsKey))
            {
                DateTime finishTime = DateTime.Now;
                TimeSpan timeTakenForRequest = finishTime - (DateTime)context.Items[StartTimeItemsKey];
                contextLogger.Information("Response for {Url} sent in {RequestTimeTaken}", context.Request.Url, timeTakenForRequest);
            }
        }

        public dynamic Invoke(NancyContext context, Exception exception)
        {
            var contextLogger = context.ResolvedRoute != null
                ? _logger.ForContext("RouteName", context.ResolvedRoute.Description.Name)
                    .ForContext("RoutePath", context.ResolvedRoute.Description.Path)
                : _logger;
            contextLogger.Error(exception, "Error occured processing request at {Url}", context.Request.Url);
            return null;
        }
    }
}