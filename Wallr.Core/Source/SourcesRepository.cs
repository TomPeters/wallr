using System.Collections.Generic;
using Wallr.Common;
using Wallr.Core.Serialization;
using Wallr.Platform;

namespace Wallr.Core.Source
{
    public interface ISourcesRepository
    {
        IEnumerable<IImageSourceConfiguration> LoadSourceConfigurations();
        void SaveSourceConfigurations(IEnumerable<IImageSourceConfiguration> configurations);
    }

    public class SourcesRepository : ISourcesRepository
    {
        private const string SourcesKey = "sources";
        private readonly IPlatform _platform;
        private readonly ISourceSerializer _sourceSerializer;

        public SourcesRepository(IPlatform platform, ISourceSerializer sourceSerializer)
        {
            _platform = platform;
            _sourceSerializer = sourceSerializer;
        }

        public IEnumerable<IImageSourceConfiguration> LoadSourceConfigurations()
        {
            IMaybe<string> sourceJson = _platform.LoadSettings(SourcesKey);
            return _sourceSerializer.Deserialize(sourceJson);
        }

        public void SaveSourceConfigurations(IEnumerable<IImageSourceConfiguration> configurations)
        {
            _platform.SaveSettings(SourcesKey, _sourceSerializer.Serialize(configurations));
        }
    }
}