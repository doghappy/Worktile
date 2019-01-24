using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Worktile.Common;
using Worktile.Enums.IM;
using Worktile.Models.IM;
using Worktile.Models.IM.Message;
using Worktile.ViewModels.Infrastructure;

namespace Worktile.WtControls.IM
{
    public sealed partial class MessageItem : UserControl, INotifyPropertyChanged
    {
        public MessageItem()
        {
            InitializeComponent();
            DisplayName = "aaa";
        }

        public event PropertyChangedEventHandler PropertyChanged;

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

        private Message _message;
        public Message Message
        {
            get => _message;
            set
            {
                if (_message != value)
                {
                    _message = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Message)));
                }
            }
        }

    }
}
