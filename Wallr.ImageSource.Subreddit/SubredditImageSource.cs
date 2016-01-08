using Serilog;

namespace Wallr.ImageSource.Subreddit
{
    public class SubredditImageSource : IImageSource
    {
        private readonly ILogger _logger;

        public SubredditImageSource(ILogger logger)
        {
            _logger = logger;
        }

        public IImage FetchLatestImage()
        {
            _logger.Information("Fetching latest image from subreddit");
            // NoCommit
            throw new System.NotImplementedException();
        }
    }
}