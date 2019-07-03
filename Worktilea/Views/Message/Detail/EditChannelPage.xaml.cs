using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Worktile.Common;
using Worktile.Common.Extensions;
using Worktile.Models.Message.Session;
using Worktile.Services;

namespace Worktile.Views.Message.Detail
{
    public sealed partial class EditChannelPage : Page, INotifyPropertyChanged
    {
        public EditChannelPage()
        {
            InitializeComponent();
            _channelMessageService = new ChannelMessageService();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        readonly ChannelMessageService _channelMessageService;
        private ChannelSession _session;
        private ChannelDetailPage _detailPage;

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

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            _detailPage = this.GetParent<ChannelDetailPage>();
            _session = _detailPage.Session;
            ChannelName = _session.Name;
            Color = _session.Color;
            Description = _session.Desc;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            IsActive = true;
            if (string.IsNullOrWhiteSpace(ChannelName))
            {
                ErrorText = "“群组名称”不能为空。";
                ShowError = true;
            }
            else
            {
                bool result = await _channelMessageService.UpdateChannelAsync(_session.Id, ChannelName, Description, Color);
                if (result)
                {
                    _session.Name = ChannelName;
                    _session.Color = Color;
                    _session.Desc = Description;
                    _session.ForShowAvatar();
                    _detailPage.ContentFrameGoBack(2);
                }
                else
                {
                    ErrorText = "修改失败";
                    ShowError = true;
                }
            }
            IsActive = false;
        }
    }
}
