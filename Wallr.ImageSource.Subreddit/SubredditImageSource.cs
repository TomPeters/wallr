using System.Collections.Generic;

namespace Wallr.ImageSource.Subreddit
{
    public class SubredditImageSource : IImageSource
    {
        public ImageSourceType SourceType => new ImageSourceType("Subreddit");
        public IDictionary<string, string> DefaultSettings => new Dictionary<string, string>();
    }
}