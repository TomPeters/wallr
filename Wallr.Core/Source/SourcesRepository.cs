using System.Collections.Generic;
using System.Linq;
using Serilog;
using Wallr.Common;
using Wallr.Core.Serialization;
using Wallr.Platform;

namespace Wallr.Core.Source
{
    public interface ISourceConfigurationsProvider
    {
        IReadOnlyList<IImageSourceConfiguration> SourceConfigurations { get; }
    }

    public interface ISourcesRepository : ISourceConfigurationsProvider
    {
        void AddSourceConfiguration(IImageSourceConfiguration configuration);
    }

    public class SourcesRepository : ISourcesRepository
    {
        private const string SourcesKey = "sources";
        private readonly IPlatform _platform;
        private readonly ISourceSerializer _sourceSerializer;
        private readonly ILogger _logger;
        private IReadOnlyList<IImageSourceConfiguration> _imageSourceConfigurations;

        public SourcesRepository(IPlatform platform, ISourceSerializer sourceSerializer, ILogger logger)
        {
            _platform = platform;
            _sourceSerializer = sourceSerializer;
            _logger = logger;
        }

        public IReadOnlyList<IImageSourceConfiguration> SourceConfigurations => 
            _imageSourceConfigurations ?? (_imageSourceConfigurations = LoadImageSourceConfigurations());

        private IReadOnlyList<IImageSourceConfiguration> LoadImageSourceConfigurations()
        {
            IMaybe<string> sourceJson = _platform.LoadSettings(SourcesKey);
            IMaybe<IReadOnlyList<IImageSourceConfiguration>> deserializedConfigurations = sourceJson.Safe(_sourceSerializer.Deserialize).Safe(c => c.ToList());
            deserializedConfigurations.Do(
                c =>_logger.Information("Loaded {PersistedSourcesCount} sources", c.Count), 
                () => _logger.Warning("Could not load any sources"));
            return deserializedConfigurations.Or(new List<IImageSourceConfiguration>()).ToList();
        }

        public void AddSourceConfiguration(IImageSourceConfiguration configuration)
        {
            _logger.Information("Added {SourceConfigurationId} to collection", configuration.Id);
            _imageSourceConfigurations = SourceConfigurations.Concat(new [] { configuration }).ToList();
            _platform.SaveSettings(SourcesKey, _sourceSerializer.Serialize(_imageSourceConfigurations));
            _logger.Information("Persisted {PersistedSourcesCount}", _imageSourceConfigurations.Count);
        }
    }
}