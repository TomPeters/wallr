using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Wallr.Core.Source
{
    public interface IImageSourceConfigurationProvider
    {
        IEnumerable<IImageSourceConfiguration> LatestImageSourceConfigurations { get; }
    }

    public class ObservableSourcesRepository : IImageSourceConfigurationProvider, IDisposable
    {
        private readonly INewImageSourceConfigurationEvents _newImageSourceConfigurationEvent;
        private readonly ISourcesRepository _sourcesRepository;
        private readonly BehaviorSubject<IEnumerable<IImageSourceConfiguration>> _imageSourceConfigurationsSubject = new BehaviorSubject<IEnumerable<IImageSourceConfiguration>>(Enumerable.Empty<IImageSourceConfiguration>());
        private IDisposable _eventsSubscription;

        public ObservableSourcesRepository(INewImageSourceConfigurationEvents newImageSourceConfigurationEvent, ISourcesRepository sourcesRepository)
        {
            _newImageSourceConfigurationEvent = newImageSourceConfigurationEvent;
            _sourcesRepository = sourcesRepository;
        }

        public void ConnectRepository()
        {
            _eventsSubscription?.Dispose();
            var newImageSourceConfigurationEvents = _newImageSourceConfigurationEvent.Events.Select(e => new ImageSourceConfiguration(new ConfiguredImageSourceId(), e.SourceType, new Dictionary<string, string>())).Publish().RefCount();
            _eventsSubscription = _sourcesRepository.LoadSourceConfigurations().ToObservable()
                .Merge(newImageSourceConfigurationEvents)
                .Scan(Enumerable.Empty<IImageSourceConfiguration>(), (enumerable, configuration) => enumerable.Concat(new[] { configuration }))
                .Subscribe(_imageSourceConfigurationsSubject.OnNext);
        }

        public IObservable<IEnumerable<IImageSourceConfiguration>> ImageSourceConfigurations => _imageSourceConfigurationsSubject;

        public IEnumerable<IImageSourceConfiguration> LatestImageSourceConfigurations => _imageSourceConfigurationsSubject.Value;

        public void Dispose()
        {
            _eventsSubscription?.Dispose();
            _imageSourceConfigurationsSubject.Dispose();
        }
    }
}