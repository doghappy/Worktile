﻿using System.ComponentModel;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Worktile.Models.IM;

namespace Worktile.WtControls
{
    public sealed partial class WtAvatar : UserControl, INotifyPropertyChanged
    {
        public WtAvatar()
        {
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private double _avatarSize;
        public double AvatarSize
        {
            get => _avatarSize;
            set
            {
                if (_avatarSize != value)
                {
                    _avatarSize = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AvatarSize)));
                }
            }
        }

        private ImageSource _profilePicture;
        public ImageSource ProfilePicture
        {
            get => _profilePicture;
            set
            {
                if (_profilePicture != value)
                {
                    _profilePicture = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ProfilePicture)));
                }
            }
        }

        private string _displayName;
        public string DisplayName
        {
            get => _displayName;
            set
            {
                if (_displayName != value)
                {
                    _displayName = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DisplayName)));
                }
            }
        }

        private string _initials;
        public string Initials
        {
            get => _initials;
            set
            {
                if (_initials != value)
                {
                    _initials = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Initials)));
                }
            }
        }

        private Color _avatarBackground;
        public Color AvatarBackground
        {
            get => _avatarBackground;
            set
            {
                if (_avatarBackground != value)
                {
                    _avatarBackground = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AvatarBackground)));
                }
            }
        }

        private ChatType _chatType;
        public ChatType ChatType
        {
            get => _chatType;
            set
            {
                if (_chatType != value)
                {
                    _chatType = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ChatType)));
                }
            }
        }

    }
}