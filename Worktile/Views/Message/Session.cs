﻿using System;
using System.ComponentModel;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Worktile.Views.Message
{
    public class Session : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string Id { get; set; }
        public string DisplayName { get; set; }
        public string Initials { get; set; }
        public BitmapImage ProfilePicture { get; set; }
        public SolidColorBrush Background { get; set; }
        public SessionType Type { get; set; }
        public bool Starred { get; set; }
        public DateTime LatestMessageAt { get; set; }
        public int Show { get; set; }

        private int _unRead;
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

        public string NamePinyin { get; set; }
        public string Name { get; set; }
        public bool IsBot { get; set; }
        public int? Component { get; set; }
        public bool IsAssistant => Component.HasValue;
        public FontFamily AvatarFont { get; set; }
        public string DefaultIcon { get; set; }
        public string Uid { get; set; }
    }

    public enum SessionType
    {
        Channel,
        Group,
        Session
    }
}
