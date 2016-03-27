using System.Collections.Generic;
using System.Linq;
using Wallr.Common;
using Wallr.Core.Serialization;
using Wallr.Platform;

namespace Wallr.Core.Source
{
    public interface IReadOnlySourcesRepository
    {
        IReadOnlyList<IImageSourceConfiguration> SourceConfigurations { get; }
    }

    public interface ISourcesRepository : IReadOnlySourcesRepository
    {
        void SaveSourceConfigurations(IEnumerable<IImageSourceConfiguration> configurations);
    }

    public class SourcesRepository : ISourcesRepository
    {
        private const string SourcesKey = "sources";
        private readonly IPlatform _platform;
        private readonly ISourceSerializer _sourceSerializer;
        private IReadOnlyList<IImageSourceConfiguration> _imageSourceConfigurations;

        public SourcesRepository(IPlatform platform, ISourceSerializer sourceSerializer)
        {
            _platform = platform;
            _sourceSerializer = sourceSerializer;
        }

        public IReadOnlyList<IImageSourceConfiguration> SourceConfigurations => 
            _imageSourceConfigurations ?? (_imageSourceConfigurations = LoadImageSourceConfigurations());

        private IReadOnlyList<IImageSourceConfiguration> LoadImageSourceConfigurations()
        {
            IMaybe<string> sourceJson = _platform.LoadSettings(SourcesKey);
            return sourceJson.Safe(_sourceSerializer.Deserialize).Or(Enumerable.Empty<IImageSourceConfiguration>()).ToList();
        }

        public void SaveSourceConfigurations(IEnumerable<IImageSourceConfiguration> configurations)
        {
            _imageSourceConfigurations = configurations.ToList();
            _platform.SaveSettings(SourcesKey, _sourceSerializer.Serialize(_imageSourceConfigurations));
        }
    }
}