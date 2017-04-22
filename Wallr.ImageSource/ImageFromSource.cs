namespace Wallr.ImageSource
{
    public interface IImageFromSource
    {
        IImage Image { get; }
        ImageSourceId SourceId { get; }
    }

    public class ImageFromSource : IImageFromSource
    {
        public ImageFromSource(IImage image, ImageSourceId sourceId)
        {
            Image = image;
            SourceId = sourceId;
        }

        public IImage Image { get; }
        public ImageSourceId SourceId { get; }
    }
}