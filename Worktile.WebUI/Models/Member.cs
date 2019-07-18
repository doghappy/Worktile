using Newtonsoft.Json;

namespace Worktile.WebUI.Models
{
    public class Member
    {
        [JsonProperty("uid")]
        public string Id { get; set; }

        public string Name { get; set; }

        [JsonProperty("display_name")]
        public string DisplayName { get; set; }

        [JsonProperty("display_name_pinyin")]
        public string DisplayNamePinYin { get; set; }

        public string Email { get; set; }

        public string Mobile { get; set; }

        public string Title { get; set; }

        public string Avatar { get; set; }

        public UserRoleType Role { get; set; }
    }

    public enum UserStatus
    {
        Ok = 1,
        Disabled,
        Init,
        Pending
    }

    public enum UserRoleType
    {
        Member = 1,
        Admin,
        Guest,
        Owner,
        Bot
    }
}
