using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Worktile.ApiModels;
using Worktile.ApiModels.ApiTeamChats;
using Worktile.Common;
using Worktile.Common.WtRequestClient;
using Worktile.Enums;

namespace Worktile.Views.Message.Dialog
{
    public sealed partial class CreateGroupDialog : ContentDialog, INotifyPropertyChanged
    {
        public CreateGroupDialog()
        {
            InitializeComponent();
            SelectedAvatars = new ObservableCollection<TethysAvatar>();
            WtVisibilities = new List<WtVisibility>
            {
                new WtVisibility
                {
                    Visibility = Enums.Visibility.Private,
                    Text = "私有：只有加入的成员才能看见此群组",
                },
                new WtVisibility
                {
                    Visibility = Enums.Visibility.Public,
                    Text = "公开：企业所有成员都可以看见此群组",
                }
            };
            SelectedVisibility = WtVisibilities.First();
            Color = WtColorHelper.Map.First().NewColor;
            MessageVisibility = Windows.UI.Xaml.Visibility.Collapsed;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public event Action<Channel> OnCreateSuccess;

        private string _color;
        public string Color
        {
            get => _color;
            set
            {
                if (_color != value)
                {
                    _color = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Color)));
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

        private string _groupName;
        public string GroupName
        {
            get => _groupName;
            set
            {
                if (_groupName != value)
                {
                    _groupName = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(GroupName)));
                }
            }
        }

        List<WtVisibility> WtVisibilities { get; }

        public ObservableCollection<TethysAvatar> SelectedAvatars { get; }

        WtVisibility _selectedVisibility;
        WtVisibility SelectedVisibility
        {
            get => _selectedVisibility;
            set
            {
                if (_selectedVisibility != value)
                {
                    _selectedVisibility = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedVisibility)));
                }
            }
        }

        private string _message;
        public string Message
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

        private Windows.UI.Xaml.Visibility _messageVisibility;
        public Windows.UI.Xaml.Visibility MessageVisibility
        {
            get => _messageVisibility;
            set
            {
                if (_messageVisibility != value)
                {
                    _messageVisibility = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MessageVisibility)));
                }
            }
        }

        private SolidColorBrush _messageColor;
        public SolidColorBrush MessageColor
        {
            get => _messageColor;
            set
            {
                if (_messageColor != value)
                {
                    _messageColor = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MessageColor)));
                }
            }
        }

        private async void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            var uids = SelectedAvatars.Select(a => a.Id);

            string groupName = GroupName.Trim();
            if (string.IsNullOrEmpty(groupName))
            {
                args.Cancel = true;
                Message = "请输入群组名称";
                MessageColor = Application.Current.Resources["WarningBrush"] as SolidColorBrush;
                MessageVisibility = Windows.UI.Xaml.Visibility.Visible;
                return;
            }
            else
            {
                MessageVisibility = Windows.UI.Xaml.Visibility.Collapsed;
            }

            var reqData = new
            {
                name = groupName,
                color = Color,
                desc = Description,
                default_uids = string.Join(',', uids),
                visibility = SelectedVisibility.Visibility
            };
            var client = new WtHttpClient();
            var data = await client.PostAsync<ApiDataResponse<Channel>>("/api/channel", reqData);
            if (data.Code == 200)
            {
                OnCreateSuccess?.Invoke(data.Data);
            }
            else
            {
                args.Cancel = true;
            }
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }

    class WtVisibility
    {
        public Enums.Visibility Visibility { get; set; }
        public string Text { get; set; }
    }
}
