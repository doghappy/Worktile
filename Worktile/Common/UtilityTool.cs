using System;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace Worktile.Common
{
    public static class UtilityTool
    {
        public async static Task<BitmapImage> GetImageFromBytesAsync(byte[] bytes)
        {
            var img = new BitmapImage();
            using (var stream = new InMemoryRandomAccessStream())
            {
                await stream.WriteAsync(bytes.AsBuffer());
                stream.Seek(0);
                await img.SetSourceAsync(stream);
            }
            return img;
        }
    }
}
