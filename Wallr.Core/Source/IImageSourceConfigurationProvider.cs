using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Wallr.Core.Source
{
    public interface IImageSourceConfigurationProvider
    {
        IReadOnlyList<IImageSourceConfiguration> LatestImageSourceConfigurations { get; }
    }

    public class ObservableSourcesRepository : IImageSourceConfigurationProvider, IDisposable
    {
        private readonly INewImageSourceConfigurationEvents _newImageSourceConfigurationEvent;
        private readonly ISourcesRepository _sourcesRepository;
        private readonly BehaviorSubject<IReadOnlyList<IImageSourceConfiguration>> _imageSourceConfigurationsSubject = new BehaviorSubject<IReadOnlyList<IImageSourceConfiguration>>(new List<IImageSourceConfiguration>());
        private IDisposable _eventsSubscription;

        public ObservableSourcesRepository(INewImageSourceConfigurationEvents newImageSourceConfigurationEvent, ISourcesRepository sourcesRepository)
        {
            _newImageSourceConfigurationEvent = newImageSourceConfigurationEvent;
            _sourcesRepository = sourcesRepository;
        }

        public void ConnectRepository()
        {
            _eventsSubscription?.Dispose();

            IReadOnlyList<IImageSourceConfiguration> initialSourceConfigurations = _sourcesRepository.LoadSourceConfigurations().ToList();
            _imageSourceConfigurationsSubject.OnNext(initialSourceConfigurations);

            IObservable<ImageSourceConfiguration> newImageSourceConfigurationEvents = _newImageSourceConfigurationEvent.Events.Select(e => new ImageSourceConfiguration(new ConfiguredImageSourceId(), e.SourceType, new Dictionary<string, string>())).Publish().RefCount();
            _eventsSubscription = newImageSourceConfigurationEvents
                .Scan<IImageSourceConfiguration, IEnumerable<IImageSourceConfiguration>>(initialSourceConfigurations, (enumerable, configuration) => enumerable.Concat(new[] { configuration }))
                .Select(e => e.ToList())
                .Subscribe(configs =>
                {
                    _sourcesRepository.SaveSourceConfigurations(configs);
                    _imageSourceConfigurationsSubject.OnNext(configs);
                });
        }

        public IObservable<IReadOnlyList<IImageSourceConfiguration>> ImageSourceConfigurations => _imageSourceConfigurationsSubject;

        public IReadOnlyList<IImageSourceConfiguration> LatestImageSourceConfigurations => _imageSourceConfigurationsSubject.Value;

        public void Dispose()
        {
            _eventsSubscription?.Dispose();
            _imageSourceConfigurationsSubject.Dispose();
        }
    }
}