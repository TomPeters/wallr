using System.Runtime.Serialization;

namespace Wallr.ImageSource.Persistence
{
    public interface IImageSourceConverter
    {
        ImageSourceConfiguration FromSerializationModel(SImageSource serializationModel);
        SImageSource ToSerializationModel(ImageSourceConfiguration imageSource);
    }

    public class ImageSourceConverter : IImageSourceConverter
    {
        public ImageSourceConfiguration FromSerializationModel(SImageSource serializationModel)
        {
            return new ImageSourceConfiguration(new ImageSourceId(serializationModel.Id),
                new ImageSourceName(serializationModel.Name),
                new ImageSourceType(serializationModel.Type),
                new ImageSourceSettings(), //nocommit settings
                serializationModel.UpdateInterval,
                serializationModel.IsEnabled);
        }

        public SImageSource ToSerializationModel(ImageSourceConfiguration imageSource)
        {
            return new SImageSource(imageSource.ImageSourceId.Value,
                imageSource.ImageSourceName.Value,
                imageSource.SourceType.Value,
                imageSource.UpdateInterval,
                imageSource.IsEnabled);
        }
    }
}