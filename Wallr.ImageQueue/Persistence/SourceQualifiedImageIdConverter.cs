using Wallr.ImagePersistence;
using Wallr.ImageSource;

namespace Wallr.ImageQueue.Persistence
{
    public interface ISourceQualifiedImageIdConverter
    {
        SourceQualifiedImageId FromSerializationModel(SSourceQualifiedImageId serializationModel);
        SSourceQualifiedImageId ToSerializationModel(SourceQualifiedImageId imageId);
    }

    public class SourceQualifiedImageIdConverter : ISourceQualifiedImageIdConverter
    {
        public SourceQualifiedImageId FromSerializationModel(SSourceQualifiedImageId serializationModel)
        {
            return new SourceQualifiedImageId(new ImageSourceId(serializationModel.Source), new ImageId(serializationModel.Id));
        }

        public SSourceQualifiedImageId ToSerializationModel(SourceQualifiedImageId imageId)
        {
            return new SSourceQualifiedImageId(imageId.SourceId.Value, imageId.ImageId.Value);
        }
    }
}