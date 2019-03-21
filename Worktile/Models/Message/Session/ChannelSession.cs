using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Worktile.Common.JsonConverters;
using Worktile.Enums;
using Worktile.Enums.Message;

namespace Worktile.Models.Message.Session
{
    public class ChannelSession : ISession, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("name_pinyin")]
        public string NamePinyin { get; set; }

        public TethysAvatar TethysAvatar { get; set; }

        private bool _starred;
        [JsonProperty("starred")]
        public bool Starred
        {
            get => _starred;
            set
            {
                if (_starred != value)
                {
                    _starred = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Starred)));
                }
            }
        }

        [JsonProperty("latest_message_at")]
        [JsonConverter(typeof(SafeUnixDateTimeConverter))]
        public DateTime LatestMessageAt { get; set; }

        [JsonProperty("show")]
        public int Show { get; set; }

        private int _unRead;
        [JsonProperty("unread")]
        public int UnRead
        {
            get => _unRead;
            set
            {
                if (_unRead != value)
                {
                    _unRead = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UnRead)));
                }
            }
        }

        [JsonProperty("created_at")]
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("team")]
        public string TeamId { get; set; }

        [JsonProperty("desc")]
        public string Desc { get; set; }

        [JsonProperty("color")]
        public string Color { get; set; }

        [JsonProperty("created_by")]
        public Member.Member CreatedBy { get; set; }

        [JsonProperty("members")]
        public ObservableCollection<Member.Member> Members { get; set; }

        [JsonProperty("is_system")]
        public bool IsSystem { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("visibility")]
        public WtVisibility Visibility { get; set; }

        [JsonProperty("disabled")]
        public bool Disabled { get; set; }

        [JsonProperty("joined")]
        public bool Joined { get; set; }

        [JsonProperty("latest_message_id")]
        public string LatestMessageId { get; set; }

        public PageType PageType => PageType.Channel;
    }
}
