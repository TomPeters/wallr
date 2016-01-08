using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Policy;
using Newtonsoft.Json.Linq;
using Serilog;

namespace Wallr.ImageSource.Subreddit
{
    public class SubredditImageSource : IImageSource
    {
        private readonly string _subreddit;
        private readonly ILogger _logger;

        public SubredditImageSource(string subreddit, ILogger logger)
        {
            _subreddit = subreddit;
            _logger = logger.ForContext("Subreddit", subreddit);
        }

        private Uri Uri => new Uri($"http://www.reddit.com/r/{_subreddit}/hot/.json");

        public IImage FetchLatestImage()
        {
            _logger.Information("Fetching latest image from subreddit");
            using (var client = new WebClient())
            {
                var jsonResponse = client.DownloadString(Uri);
                _logger.Information("Received response from reddit");
                var jObject = JObject.Parse(jsonResponse);
                return jObject["data"]["children"].Value<JArray>().Select(j => j["data"]).Skip(RandomSkip).Select(TryDownloadImage).First(d => d.IsSuccessful).Image;
            }
        }

        // FIXME: Random to prove that it works, this should be removed
        private readonly Random _random = new Random();
        private int RandomSkip => _random.Next(0, 5);

        private ImageDownloadResult TryDownloadImage(JToken postJson)
        {
            string url = postJson["url"].Value<string>();
            if (url.EndsWith("jpg"))
            {
                return new ImageDownloadResult
                {
                    IsSuccessful = true,
                    Image = new RemoteImage(url)
                };
            }
            return new ImageDownloadResult
            {
                IsSuccessful = false
            };
        }

        private class ImageDownloadResult
        {
            public bool IsSuccessful { get; set; }
            public IImage Image { get; set; }
        }
    }

    public class RemoteImage : IImage
    {
        private readonly string _url;

        public RemoteImage(string url)
        {
            _url = url;
        }

        public ImageId Id => new ImageId(_url);
        public string FileName => _url.Split('/').Last();
        public Stream FileStream => new WebClient().OpenRead(new Uri(_url));
    }
}