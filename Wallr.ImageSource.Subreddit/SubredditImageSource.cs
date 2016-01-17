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

        private Uri BaseUri => new Uri($"http://www.reddit.com/r/{_subreddit}/top/.json");

        public IEnumerable<ISourceImage> Images
        {
            get
            {
                _logger.Information("Fetching latest image from subreddit");
                using (WebClient client = new WebClient())
                {
                    foreach (ISourceImage image in ImagesFromUri(client, BaseUri))
                        yield return image;
                }
            }
        }

        private IEnumerable<ISourceImage> ImagesFromUri(WebClient client, Uri uri, int currentCount = 0)
        {
            string jsonResponse = client.DownloadString(uri);
            _logger.Information("Received response from reddit");
            JObject jObject = JObject.Parse(jsonResponse);
            JToken dataToken = jObject["data"];
            IReadOnlyList<JToken> childrenTokens = dataToken["children"].Value<JArray>().Select(j => j["data"]).ToList();
            IEnumerable<ISourceImage> images = childrenTokens.Select(TryDownloadImage).Where(i => i.IsSuccessful).Select(i => i.Image);
            foreach (ISourceImage image in images)
                yield return image;

            string afterKey = dataToken["after"].Value<string>();
            int nextCount = currentCount + 25;
            Uri nextUri = new Uri(BaseUri + $"?count={nextCount}&after={afterKey}");
            foreach (ISourceImage image in ImagesFromUri(client, nextUri, nextCount))
                yield return image;
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
            public ISourceImage Image { get; set; }
        }
    }

    public class RemoteImage : ISourceImage
    {
        private readonly string _url;

        public RemoteImage(string url)
        {
            _url = url;
        }

        public LocalImageId ImageId => new LocalImageId(FileName);
        private string FileName => _url.Split('/').Last().Split('.').First();
        public Stream FileStream => new WebClient().OpenRead(new Uri(_url));
    }
}