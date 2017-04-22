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
            return null; // nocommit
        }

        public SImageSource ToSerializationModel(ImageSourceConfiguration imageSource)
        {
            return new SImageSource(imageSource.ImageSourceId.Value);
        }
    }
}