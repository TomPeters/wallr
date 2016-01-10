namespace Wallr.ImageSource.Subreddit
{
    public class SubredditImageSourceId : ImageSourceId
    {
        public SubredditImageSourceId(string subredditName) : base($"Subreddit-{subredditName}")
        {
        }
    }
}