using ImageMagick;

namespace Cooking.Infrastructure
{
    public interface ImagingService
    {
        byte[] GetThumbnail(byte[] fileBytes, int size);
    }

    public class MagickImagingService : ImagingService
    {
        public byte[] GetThumbnail(byte[] fileBytes, int size)
        {
            using var image = new MagickImage(fileBytes);
            var mainSize = new MagickGeometry(size);
            image.Resize(mainSize);
            return image.ToByteArray();
        }
    }
}