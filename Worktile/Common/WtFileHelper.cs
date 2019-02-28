namespace Worktile.Common
{
    static class WtFileHelper
    {
        public static string GetFileIcon(string ext)
        {
            string name = ext;
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
            }
            return $"{DataSource.ApiUserMeData.Config.CdnRoot}image/icons/{name}.png";
        }
    }
}
