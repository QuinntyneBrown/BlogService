using System.Drawing.Imaging;
using System.Linq;

namespace BlogService.Features.DigitalAssets
{
    public static class ImageExtensions
    {
        public static string GetMimeType(this System.Drawing.Image image)
        {
            return image.RawFormat.GetMimeType();
        }

        public static string GetMimeType(this ImageFormat imageFormat)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            return codecs.First(codec => codec.FormatID == imageFormat.Guid).MimeType;
        }
    }
}
