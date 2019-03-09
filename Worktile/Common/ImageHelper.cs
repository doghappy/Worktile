using System;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace Worktile.Common
{
    public static class ImageHelper
    {
        public async static Task<BitmapImage> GetImageAsync(IBuffer buffer)
        {
            var img = new BitmapImage();
            using (var stream = new InMemoryRandomAccessStream())
            {
                await stream.WriteAsync(buffer);
                stream.Seek(0);
                await img.SetSourceAsync(stream);
            }
            return img;
        }
    }
}
