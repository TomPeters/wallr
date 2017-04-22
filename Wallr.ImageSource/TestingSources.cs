using System;
using System.Collections.Generic;

namespace Wallr.ImageSource
{
    public class TestingSources // nocommit, remove this once more features are in place
    {
        public IEnumerable<IImageSourceConfiguration> GetTestingSourceConfigurations()
        {
            yield return new ImageSourceConfiguration(new ImageSourceId(Guid.NewGuid()), new ImageSourceName("Wallpapers subreddit"), new ImageSourceType("Subreddit"), new ImageSourceSettings(),
                TimeSpan.FromSeconds(20), true);
        }
    }
}