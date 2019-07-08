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
                    case "doc":
                    case "docx":
                        name = "doc";
                        break;
                    case "ppt":
                    case "pptx":
                        name = "ppt";
                        break;
                    case "xls":
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
                    case "apk":
                        name = "apk";
                        break;
                    case "bak":
                        name = "bak";
                        break;
                    case "box":
                        name = "box";
                        break;
                    case "cs":
                        name = "cs";
                        break;
                    case "csv":
                        name = "csv";
                        break;
                    case "evernote":
                        name = "evernote";
                        break;
                    case "exe":
                        name = "exe";
                        break;
                    case "fla":
                        name = "fla";
                        break;
                    case "html":
                        name = "html";
                        break;
                    case "ipa":
                        name = "ipa";
                        break;
                    case "java":
                    case "jsp":
                        name = "java";
                        break;
                    case "js":
                        name = "js";
                        break;
                    case "mp3":
                        name = "mp3";
                        break;
                    case "mp4":
                        name = "mp4";
                        break;
                    case "onedrive":
                        name = "onedrive";
                        break;
                    case "onenote":
                        name = "onenote";
                        break;
                    case "page":
                        name = "page";
                        break;
                    case "pdf":
                        name = "pdf";
                        break;
                    case "php":
                        name = "php";
                        break;
                    case "processon":
                        name = "processon";
                        break;
                    case "quip":
                        name = "quip";
                        break;
                    case "rar":
                        name = "rar";
                        break;
                    case "shimo":
                        name = "shimo";
                        break;
                    case "snippet":
                        name = "snippet";
                        break;
                    case "swf":
                        name = "swf";
                        break;
                    case "ttf":
                        name = "ttf";
                        break;
                    case "txt":
                        name = "txt";
                        break;
                    case "vss":
                        name = "vss";
                        break;
                    case "xsd":
                        name = "xsd";
                        break;
                    case "yinxiang":
                        name = "yinxiang";
                        break;
                    case "youdaonote":
                        name = "youdaonote";
                        break;
                    case "zip":
                        name = "zip";
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
