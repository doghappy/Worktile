using Newtonsoft.Json;
using System.ComponentModel;
using Worktile.Models.Privilege;

namespace Worktile.Models
{
    public class WtApp : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("integration_id")]
        public string IntegrationId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("display_name")]
        public string DisplayName { get; set; }

        [JsonProperty("privileges")]
        public PrivilegeObject Privileges { get; set; }

        public string Icon { get; set; }

        public string Icon2 { get; set; }

        private int _unreadCount;
        public int UnreadCount
        {
            get => _unreadCount;
            set
            {
                if (_unreadCount != value)
                {
                    _unreadCount = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UnreadCount)));
                }
            }
        }
    }
}
