using System;
using System.Collections.Async;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Serilog;
using Wallr.ImageSource.Api;

namespace Wallr.ImageSource.Subreddit
{
    public interface ISubredditSettings
    {
        string SubredditName { get; }
    }

    public class SubredditImageSource : IImageSource<ISubredditSettings>
    {
        private readonly ILogger _logger;
        private readonly HttpClient _httpClient;

        public SubredditImageSource(ILogger logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
        }

        public string SourceTypeName => "Subreddit";
        private static Uri BaseUri(string subredditName) => new Uri($"http://www.reddit.com/r/{subredditName}/top/.json");

        public IAsyncEnumerable<IImage> GetImages(ISubredditSettings settings)
        {
            return ImagesFromUri(BaseUri(/*settings.SubredditName*/"wallpaper")); // nocommit, change to the setting once settings are supported
        }

        private IAsyncEnumerable<IImage> ImagesFromUri(Uri baseUri, int currentCount = 0)
        {
            return new AsyncEnumerable<IImage>(async yield =>
            {
                HttpResponseMessage jsonResponse = await _httpClient.GetAsync(baseUri);
                _logger.Information("Received response from reddit");
                JObject jObject = JObject.Parse(await jsonResponse.Content.ReadAsStringAsync());
                JToken dataToken = jObject["data"];
                IReadOnlyList<JToken> childrenTokens = dataToken["children"].Value<JArray>().Select(j => j["data"]).ToList();
                if (childrenTokens.Count == 0)
                    yield.Break();
                IEnumerable<IImage> images = childrenTokens.Select(t => t["url"].Value<string>())
                    .Where(url => url.EndsWith("jpg"))
                    .Select(url => new Uri(url))
                    .Select(url => new RemoteImage(url, _httpClient));
                foreach (IImage image in images)
                    await yield.ReturnAsync(image);

                var afterKey = dataToken["after"].Value<string>();
                int nextCount = currentCount + 25;
                var nextUri = new Uri($"{baseUri}?count={nextCount}&after={afterKey}");
                await ImagesFromUri(nextUri, nextCount).ForEachAsync(yield.ReturnAsync);
            });
        }

        private class RemoteImage : IImage
        {
            private readonly Uri _url;
            private readonly HttpClient _httpClient;

            public RemoteImage(Uri url, HttpClient httpClient)
            {
                _url = url;
                _httpClient = httpClient;
            }

            public Task<string> Id => Task.FromResult(_url.ToString().Split('/').Last().Split('.').First());
            public async Task<Stream> GetImageStream()
            {
                HttpResponseMessage response = await _httpClient.GetAsync(_url);
                return await response.Content.ReadAsStreamAsync();
            }
        }
    }
}