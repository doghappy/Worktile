namespace Worktile.Models
{
    public class Avatar
    {
        public string DisplayName { get; set; }
        public string ProfilePicture { get; set; }
        public string Initials
        {
            get
            {
                if (string.IsNullOrWhiteSpace(DisplayName))
                    return string.Empty;
                else if (DisplayName.Length < 3)
                    return DisplayName;
                else
                    return DisplayName.Substring(DisplayName.Length - 2, 2);
            }
        }
    }
}
