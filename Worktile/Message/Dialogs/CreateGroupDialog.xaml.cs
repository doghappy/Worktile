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

        private bool _showError;
        public bool ShowError
        {
            get => _showError;
            set
            {
                if (_showError != value)
                {
                    _showError = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ShowError)));
                }
            }
        }

        private string _errorText;
        public string ErrorText
        {
            get => _errorText;
            set
            {
                if (_errorText != value)
                {
                    _errorText = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ErrorText)));
                }
            }
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            IsActive = true;
            ShowError = false;
            if (string.IsNullOrWhiteSpace(ChannelName))
            {
                ErrorText = UtilityTool.GetStringFromResources("ThisOptionIsRequired");
                ShowError = true;
                GroupNameTextBox.Focus(FocusState.Programmatic);
                args.Cancel = true;
            }
            else
            {
                //    var avatars = Avatars.Select(a => a.Id);
                //    string uids = string.Join(',', avatars);
                //    var visibility = IsPrivate ? WtVisibility.Private : WtVisibility.Public;
                //    var data = await _messageService.CreateChannelAsync(uids, ChannelName.Trim(), Color, Description.Trim(), visibility);
                //    if (data.Code == 200)
                //    {
                //        for (int i = 0; i < 10; i++)
                //        {
                //            await Task.Delay(20);
                //            var firstSession = _masterPage.Sessions.First();
                //            if (firstSession.Id == data.Data.Id && _masterPage.SelectedSession != firstSession)
                //            {
                //                _masterPage.SelectedSession = firstSession;
                //                break;
                //            }
                //        }
                //    }
                //    else if (data.Code == 3004)
                //    {
                //        ErrorText = "已有同名群组";
                //        ShowError = true;
                //        ChannelNameTextBox.Focus(FocusState.Programmatic);
                //    }
                //    else
                //    {
                //        ErrorText = "创建失败";
                //        ShowError = true;
                //    }
            }
        }
    }
}
