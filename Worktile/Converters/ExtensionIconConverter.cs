using System;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;

namespace Worktile.Converters
{
    public class ExtensionIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string name = "default";
            string ext = value?.ToString();
            if (!string.IsNullOrEmpty(ext))
            {
                switch (ext)
                {
                    case "docx":
                        name = "doc";
                        break;
                    case "pptx":
                        name = "ppt";
                        break;
                    case "xlsx":
                        name = "xls";
                        break;
                    case "jpg":
                    case "jpeg":
                    case "png":
                    case "gif":
                    case "bmp":
                        name = "img";
                        break;
                    case "java":
                    case "jsp":
                        name = "java";
                        break;
                    default:
                        name = ext;
                        break;
                }
            }
            return new BitmapImage
            {
                UriSource = new Uri($"ms-appx:///Assets/Images/Icons/{name}.png")
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
