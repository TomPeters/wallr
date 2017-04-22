using System.IO;
using System.Threading.Tasks;

namespace Wallr.ImageSource
{
    public interface IImage
    {
        Task<ImageId> GetId();
        Task<Stream> GetImageStream();
    }

    public class Image : IImage
    {
        private readonly Api.IImage _apiImage;

        public Image(Api.IImage apiImage)
        {
            _apiImage = apiImage;
        }

        public async Task<ImageId> GetId()
        {
            string apiImageId = await _apiImage.Id;
            return new ImageId(apiImageId);
        }

        public Task<Stream> GetImageStream()
        {
            return _apiImage.GetImageStream();
        }
    }
}