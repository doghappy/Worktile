using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.ComponentModel;
using Worktile.Common.JsonConverters;
using Worktile.Enums.Message;

namespace Worktile.Models.Message.Session
{
    public class MemberSession : ISession, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [JsonProperty("_id")]
        public string Id { get; set; }

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

        [JsonProperty("is_bot")]
        public bool IsBot { get; set; }

        [JsonProperty("created_by")]
        public string CreatedBy { get; set; }

        [JsonProperty("to")]
        public Member.Member To { get; set; }

        [JsonProperty("component")]
        public int? Component { get; set; }

        public PageType PageType => Component.HasValue ? PageType.Assistant : PageType.Member;
    }
}
