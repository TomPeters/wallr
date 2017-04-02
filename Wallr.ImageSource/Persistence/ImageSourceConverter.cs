namespace Wallr.ImageSource.Persistence
{
    public interface IImageSourceConverter
    {
        IImageSource FromSerializationModel(SImageSource serializationModel);
        SImageSource ToSerializationModel(IImageSource imageSource);
    }

    public class ImageSourceConverter : IImageSourceConverter
    {
        public IImageSource FromSerializationModel(SImageSource serializationModel)
        {
            return null; // nocommit
        }

        public SImageSource ToSerializationModel(IImageSource imageSource)
        {
            return new SImageSource(imageSource.ImageSourceId.Value);
        }
    }
}