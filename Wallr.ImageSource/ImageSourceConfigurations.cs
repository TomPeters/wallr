using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Optional;
using Optional.Linq;
using Wallr.Common;
using Wallr.ImageSource.Persistence;
using Wallr.Platform;

namespace Wallr.ImageSource
{
    public class SourceConfigurationAddedEvent
    {
        public SourceConfigurationAddedEvent(IImageSourceConfiguration sourceConfiguration)
        {
            SourceConfiguration = sourceConfiguration;
        }

        public IImageSourceConfiguration SourceConfiguration { get; }
    }

    public class SourceConfigurationRemovedEvent
    {
        public SourceConfigurationRemovedEvent(ImageSourceId sourceId)
        {
            SourceId = sourceId;
        }

        public ImageSourceId SourceId { get; }
    }

    public class SourceConfigurationChangedEvent
    {
        public SourceConfigurationChangedEvent(IImageSourceConfiguration updatedSourceConfiguration)
        {
            UpdatedSourceConfiguration = updatedSourceConfiguration;
        }

        public IImageSourceConfiguration UpdatedSourceConfiguration { get; }
    }

    public class SourceConfigurationsLoadedEvent
    {
        public SourceConfigurationsLoadedEvent(IEnumerable<IImageSourceConfiguration> sourceConfigurations)
        {
            SourceConfigurations = sourceConfigurations;
        }

        public IEnumerable<IImageSourceConfiguration> SourceConfigurations { get; }
    }

    public interface IImageSourceConfigurations : IEnumerable<IImageSourceConfiguration> // nocommit, maybe also make it implement IObservable?
    {
        Task RehydrateSources();
        IImageSourceConfiguration Get(ImageSourceId id);
        Task Add(IImageSourceConfiguration source);
        Task Remove(ImageSourceId id);

        IObservable<SourceConfigurationAddedEvent> SourceAdds { get; }
        IObservable<SourceConfigurationChangedEvent> SourceChanges { get; }
        IObservable<SourceConfigurationRemovedEvent> SourceRemoves { get; }
        IObservable<SourceConfigurationsLoadedEvent> SourceLoads { get; }
    }

    public class ImageSourceConfigurations : IDisposable, IImageSourceConfigurations
    {
        private const string SettingsKey = "Sources";
        private readonly IPersistence _persistence;
        private readonly IImageSourceConverter _imageSourceConverter;
        private ImageSourcesCollection _sources = new ImageSourcesCollection();

        private readonly Subject<SourceConfigurationAddedEvent> _sourceAdds = new Subject<SourceConfigurationAddedEvent>();
        private readonly Subject<SourceConfigurationChangedEvent> _sourceChanges = new Subject<SourceConfigurationChangedEvent>();
        private readonly Subject<SourceConfigurationRemovedEvent> _sourceRemoves = new Subject<SourceConfigurationRemovedEvent>();
        private readonly Subject<SourceConfigurationsLoadedEvent> _sourceLoads = new Subject<SourceConfigurationsLoadedEvent>();

        public ImageSourceConfigurations(IPersistence persistence, IImageSourceConverter imageSourceConverter)
        {
            _persistence = persistence;
            _imageSourceConverter = imageSourceConverter;
        }

        public IEnumerator<IImageSourceConfiguration> GetEnumerator()
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
                .Select(sources => sources.Select(_imageSourceConverter.FromSerializationModel)) // nocommit, hard code one in here for testing purposes
                .Select(s => new ImageSourcesCollection(s))
                .ValueOr(new ImageSourcesCollection());

            _sourceLoads.OnNext(new SourceConfigurationsLoadedEvent(_sources.ToList()));
        }

        public IImageSourceConfiguration Get(ImageSourceId id)
        {
            return _sources[id];
        }

        public async Task Add(IImageSourceConfiguration source)
        {
            _sources.Add(source);
            await PersistAllSources();
            _sourceAdds.OnNext(new SourceConfigurationAddedEvent(source));
        }

        public async Task Remove(ImageSourceId id)
        {
            _sources.Remove(id);
            await PersistAllSources();
            _sourceRemoves.OnNext(new SourceConfigurationRemovedEvent(id));
        }

        public async Task Change(ImageSourceId id, Func<IImageSourceConfiguration, IImageSourceConfiguration> updateConfiguration)
        {
            var originalSource = _sources[id];
            var updatedSource = updateConfiguration(originalSource);
            _sources.Remove(id);
            _sources.Add(updatedSource);
            await PersistAllSources();
            _sourceChanges.OnNext(new SourceConfigurationChangedEvent(updatedSource));
        }

        public IObservable<SourceConfigurationAddedEvent> SourceAdds => _sourceAdds;
        public IObservable<SourceConfigurationChangedEvent> SourceChanges => _sourceChanges;
        public IObservable<SourceConfigurationRemovedEvent> SourceRemoves => _sourceRemoves;
        public IObservable<SourceConfigurationsLoadedEvent> SourceLoads => _sourceLoads;

        private Task PersistAllSources()
        {
            IEnumerable<SImageSource> sources = _sources.Select(_imageSourceConverter.ToSerializationModel);
            return _persistence.SaveSettings(SettingsKey, JsonConvert.SerializeObject(sources));
        }

        private class ImageSourcesCollection : KeyedCollection<ImageSourceId, IImageSourceConfiguration>
        {
            public ImageSourcesCollection()
                : this(Enumerable.Empty<IImageSourceConfiguration>())
            {
            }
            public ImageSourcesCollection(IEnumerable<IImageSourceConfiguration> sources)
            {
                foreach (IImageSourceConfiguration item in sources)
                    Add(item);
            }

            protected override ImageSourceId GetKeyForItem(IImageSourceConfiguration item)
            {
                return item.ImageSourceId;
            }
        }

        public void Dispose()
        {
            _sourceAdds.Dispose();
            _sourceChanges.Dispose();
            _sourceRemoves.Dispose();
            _sourceLoads.Dispose();
        }
    }
}