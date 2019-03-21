using Newtonsoft.Json;
using System;
using System.ComponentModel;
using Worktile.Common.JsonConverters;
using Worktile.Enums.Message;

namespace Worktile.Models.Message
{
    public class Message : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("from")]
        public MessageFrom From { get; set; }

        [JsonProperty("to")]
        public MessageTo To { get; set; }

        [JsonProperty("type")]
        public MessageType Type { get; set; }

        [JsonProperty("body")]
        public MessageBody Body { get; set; }

        [JsonProperty("client")]
        public int Client { get; set; }

        [JsonProperty("created_at")]
        [JsonConverter(typeof(SafeUnixDateTimeConverter))]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("component")]
        public int Component { get; set; }

        [JsonProperty("category_filter")]
        public int CategoryFilter { get; set; }

        [JsonProperty("category_trigger")]
        public int CategoryTrigger { get; set; }

        [JsonProperty("team")]
        public string Team { get; set; }

        private bool _isStar;
        [JsonProperty("is_star")]
        public bool IsStar
        {
            get => _isStar;
            set
            {
                if (_isStar != value)
                {
                    _isStar = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsStar)));
                }
            }
        }


        private bool _isPinned;
        [JsonProperty("is_pinned")]
        public bool IsPinned
        {
            get => _isPinned;
            set
            {
                if (_isPinned != value)
                {
                    _isPinned = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsPinned)));
                }
            }
        }

        private bool _isPending;
        [JsonProperty("is_pending")]
        public bool IsPending
        {
            get => _isPending;
            set
            {
                if (_isPending != value)
                {
                    _isPending = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsPending)));
                }
            }
        }

        [JsonProperty("is_unread")]
        public bool IsUnread { get; set; }

        public bool IsShowPin => Type != MessageType.Activity;
    }
}
