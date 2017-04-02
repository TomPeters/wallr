using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Optional;
using Optional.Linq;
using Wallr.Common;
using Wallr.ImageSource.Persistence;
using Wallr.Platform;

namespace Wallr.ImageSource
{
    public interface IImageSources : IEnumerable<IImageSource>
    {
        Task RehydrateSources();
        IImageSource Get(ImageSourceId id);
        Task Add(IImageSource source);
        Task Remove(ImageSourceId id);
    }

    public class ImageSources : IImageSources
    {
        private const string SettingsKey = "Sources";
        private readonly IPersistence _persistence;
        private readonly IImageSourceConverter _imageSourceConverter;
        private ImageSourcesCollection _sources = new ImageSourcesCollection();

        public ImageSources(IPersistence persistence, IImageSourceConverter imageSourceConverter)
        {
            _persistence = persistence;
            _imageSourceConverter = imageSourceConverter;
        }

        public IEnumerator<IImageSource> GetEnumerator()
        {
            return _sources.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public async Task RehydrateSources()
        {
            Option<string> persistedSettings = await _persistence.LoadSettings(SettingsKey);
            _sources = persistedSettings
                .Select(JsonConvert.DeserializeObject<IEnumerable<SImageSource>>)
                .Select(sources => sources.Select(_imageSourceConverter.FromSerializationModel))
                .Select(s => new ImageSourcesCollection(s))
                .ValueOr(new ImageSourcesCollection());
        }

        public IImageSource Get(ImageSourceId id)
        {
            return _sources[id];
        }

        public Task Add(IImageSource source)
        {
            _sources.Add(source);
            return PersistAllSources();
        }

        public Task Remove(ImageSourceId id)
        {
            _sources.Remove(id);
            return PersistAllSources();
        }

        private Task PersistAllSources()
        {
            IEnumerable<SImageSource> sources = _sources.Select(_imageSourceConverter.ToSerializationModel);
            return _persistence.SaveSettings(SettingsKey, JsonConvert.SerializeObject(sources));
        }

        private class ImageSourcesCollection : KeyedCollection<ImageSourceId, IImageSource>
        {
            public ImageSourcesCollection()
                : this(Enumerable.Empty<IImageSource>())
            {
            }
            public ImageSourcesCollection(IEnumerable<IImageSource> sources)
            {
                sources.ForEach(Add);
            }

            protected override ImageSourceId GetKeyForItem(IImageSource item)
            {
                return item.ImageSourceId;
            }
        }
    }
}