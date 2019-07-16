using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Worktile.Common;

namespace Worktile.Message.Dialogs
{
    public sealed partial class CreateGroupDialog : ContentDialog, INotifyPropertyChanged
    {
        public CreateGroupDialog()
        {
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private bool _isActive;
        public bool IsActive
        {
            get => _isActive;
            set
            {
                if (_isActive != value)
                {
                    _isActive = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsActive)));
                }
            }
        }

        private string _description;
        public string Description
        {
            get => _description;
            set
            {
                if (_description != value)
                {
                    _description = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Description)));
                }
            }
        }

        private string _channelName;
        public string ChannelName
        {
            get => _channelName;
            set
            {
                if (_channelName != value)
                {
                    _channelName = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ChannelName)));
                }
            }
        }

        private bool _isPrivate;
        public bool IsPrivate
        {
            get => _isPrivate;
            set
            {
                if (_isPrivate != value)
                {
                    _isPrivate = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsPrivate)));
                }
            }
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {

        }
    }
}
