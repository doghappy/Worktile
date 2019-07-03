namespace Worktile.Common
{
    static class WtFileHelper
    {
        public static string GetFileIcon(string ext)
        {
            string name = "default";
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
            return $"/Assets/Images/Icons/{name}.png";
        }

        /// <summary>
        /// 获取缩略图URL
        /// </summary>
        /// <param name="thumbnail">00a2c00f-b970-4560-9da9-88e2a12a2b8b_480x360.png</param>
        /// <returns></returns>
        public static string GetThumbnailUrl(string thumbnail)
        {
            return DataSource.ApiUserMeData.Config.Box.BaseUrl + "thumb/" + thumbnail;
        }

        public static string GetS3FileUrl(string id)
        {
            return $"{DataSource.ApiUserMeData.Config.Box.BaseUrl}entities/{id}/from-s3?team_id={DataSource.Team.Id}";
        }
    }
}
