using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Serialization;
using Wallr.Common;
using Wallr.Core.Source;

namespace Wallr.Core.Serialization
{
    public interface ISourceSerializer
    {
        string Serialize(IEnumerable<IImageSourceConfiguration> imageSourceConfigurations);
        IEnumerable<IImageSourceConfiguration> Deserialize(string imageSourceConfigurations);
    }

    public class SourceSerializer : ISourceSerializer
    {
        public string Serialize(IEnumerable<IImageSourceConfiguration> imageSourceConfigurations)
        {
            SourcesCollection sourceCollection = new SourcesCollection
            {
                Sources = imageSourceConfigurations.Select(c =>
                {
                    return new global::Serialization.Source()
                    {
                        Type = c.ImageSourceType.ToString(),
                        Id = c.Id.Value,
                        Settings = c.Settings.ToDictionary(p => p.Key, p => p.Value)
                    };
                }).ToList()
            };
            return JsonConvert.SerializeObject(sourceCollection);
        }

        public IEnumerable<IImageSourceConfiguration> Deserialize(string imageSourceConfigurations)
        {
            SourcesCollection sourcesCollection = JsonConvert.DeserializeObject<SourcesCollection>(imageSourceConfigurations);
            return sourcesCollection.Sources.Select(c =>
            {
                return new ImageSourceConfiguration(new ConfiguredImageSourceId(c.Id), new ImageSourceType(c.Type), 
                    c.Settings.ToDictionary(p => p.Key, p => p.Value));
            });
        }
    }
}