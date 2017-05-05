using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Wallr.ImagePersistence;
using Wallr.ImageQueue.Persistence;

namespace Wallr.ImageQueue
{
    public interface IImageQueueConverter
    {
        string Serialize(IEnumerable<SourceQualifiedImageId> queue);
        IEnumerable<SourceQualifiedImageId> Deserialize(string queueJson);
    }

    public class ImageQueueConverter : IImageQueueConverter
    {
        private readonly ISourceQualifiedImageIdConverter _converter;

        public ImageQueueConverter(ISourceQualifiedImageIdConverter converter)
        {
            _converter = converter;
        }

        public string Serialize(IEnumerable<SourceQualifiedImageId> queue)
        {
            var sQueue = queue.Select(_converter.ToSerializationModel);
            return JsonConvert.SerializeObject(sQueue);
        }

        public IEnumerable<SourceQualifiedImageId> Deserialize(string queueJson)
        {
            var sQueue = JsonConvert.DeserializeObject<IEnumerable<SSourceQualifiedImageId>>(queueJson);
            return sQueue.Select(_converter.FromSerializationModel);
        }
    }
}