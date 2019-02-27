using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.ComponentModel;
using Windows.UI.Xaml;
using Worktile.Enums.Message;
using Worktile.Views.Message;

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
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("component")]
        public int Component { get; set; }

        [JsonProperty("category_filter")]
        public int CategoryFilter { get; set; }

        [JsonProperty("category_trigger")]
        public int CategoryTrigger { get; set; }

        [JsonProperty("team")]
        public string Team { get; set; }

        [JsonProperty("is_star")]
        public bool IsStar { get; set; }

        private bool _isPinned;
        [JsonProperty("is_pinned")]
        public bool IsPinned
        {
            get => _isPinned;
            set
            {
                if (value)
                {
                    PinVisibility = Visibility.Collapsed;
                    UnPinVisibility = Visibility.Visible;
                }
                else
                {
                    PinVisibility = Visibility.Visible;
                    UnPinVisibility = Visibility.Collapsed;
                }
                if (_isPinned != value)
                {
                    _isPinned = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsPinned)));
                }
            }
        }

        [JsonProperty("is_pending")]
        public bool IsPending { get; set; }

        [JsonProperty("is_unread")]
        public bool IsUnread { get; set; }

        public bool IsShowPin => Type != MessageType.Activity;

        Visibility _pinVisibility;
        public Visibility PinVisibility
        {
            get => _pinVisibility;
            set
            {
                if (_pinVisibility != value)
                {
                    _pinVisibility = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PinVisibility)));
                }
            }
        }

        Visibility _unPinVisibility;
        public Visibility UnPinVisibility
        {
            get => _unPinVisibility;
            set
            {
                if (_unPinVisibility != value)
                {
                    _unPinVisibility = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UnPinVisibility)));
                }
            }
        }
    }
}
