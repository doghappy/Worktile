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
