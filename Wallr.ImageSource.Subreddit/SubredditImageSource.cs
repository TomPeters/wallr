using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Newtonsoft.Json.Linq;
using Serilog;
using Wallr.Interfaces;

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

        public IEnumerable<ISourceImage> Images
        {
            get
            {
                _logger.Information("Fetching latest image from subreddit");
                using (var client = new WebClient())
                {
                    var jsonResponse = client.DownloadString(Uri);
                    _logger.Information("Received response from reddit");
                    var jObject = JObject.Parse(jsonResponse);
                    var images = jObject["data"]["children"].Value<JArray>().Select(j => j["data"]).Select(TryDownloadImage).Where(i => i.IsSuccessful).Select(i => i.Image);
                    foreach (ISourceImage image in images)
                        yield return image;
                }
            }
        }

        public ImageSourceId ImageSourceId => new SubredditImageSourceId(_subreddit);

        private ImageDownloadResult TryDownloadImage(JToken postJson)
        {
            string url = postJson["url"].Value<string>();
            if (url.EndsWith("jpg"))
            {
                return new ImageDownloadResult
                {
                    IsSuccessful = true,
                    Image = new RemoteImage(url, _subreddit)
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
            public ISourceImage Image { get; set; }
        }
    }

    public class RemoteImage : ISourceImage
    {
        private readonly string _url;
        private readonly string _subredditName;

        public RemoteImage(string url, string subredditName)
        {
            _url = url;
            _subredditName = subredditName;
        }

        public LocalImageId ImageId => new LocalImageId(FileName);
        private string FileName => _url.Split('/').Last().Split('.').First();
        public Stream FileStream => new WebClient().OpenRead(new Uri(_url));
    }
}