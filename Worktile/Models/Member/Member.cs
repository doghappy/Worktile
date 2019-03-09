using Newtonsoft.Json;
using System.ComponentModel;
using Worktile.Enums;

namespace Worktile.Models.Member
{
    public class Member : IMemberBase, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [JsonProperty("team")]
        public string Team { get; set; }

        [JsonProperty("uid")]
        public string Uid { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("display_name")]
        public string DisplayName { get; set; }

        [JsonProperty("avatar")]
        public string Avatar { get; set; }

        [JsonProperty("role")]
        public RoleType Role { get; set; }

        [JsonProperty("desc")]
        public string Desc { get; set; }

        [JsonProperty("short_code")]
        public string ShortCode { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("mobile")]
        public string Mobile { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("display_name_pinyin")]
        public string DisplayNamePinyin { get; set; }

        [JsonProperty("preferences")]
        public Preference Preferences { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("department_name")]
        public string DepartmentName { get; set; }

        private TethysAvatar _tethysAvatar;
        public TethysAvatar TethysAvatar
        {
            get => _tethysAvatar;
            set
            {
                if (_tethysAvatar != value)
                {
                    _tethysAvatar = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TethysAvatar)));
                }
            }
        }
    }
}
