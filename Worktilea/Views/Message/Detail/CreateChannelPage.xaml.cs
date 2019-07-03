using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Worktile.Common;
using System.ComponentModel;
using System.Collections.ObjectModel;
using Worktile.Models;
using Worktile.Services;
using System.Linq;
using Worktile.Enums;
using System.Threading.Tasks;

namespace Worktile.Views.Message.Detail
{
    public sealed partial class CreateChannelPage : Page, INotifyPropertyChanged
    {
        public CreateChannelPage()
        {
            InitializeComponent();
            Avatars = new ObservableCollection<TethysAvatar>();
            _messageService = new MessageService();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        readonly MessageService _messageService;
        Frame _frame;
        MasterPage _masterPage;

        public ObservableCollection<TethysAvatar> Avatars { get; }

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


        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Description = string.Empty;
            ChannelNameTextBox.Focus(FocusState.Programmatic);
            _masterPage = Frame.GetParent<MasterPage>();
            _frame = _masterPage.GetChild<Frame>("MasterContentFrame");
        }

        private void GoBackButton_Click(object sender, RoutedEventArgs e)
        {
            if (_frame.CanGoBack)
            {
                _frame.GoBack();
            }
            else
            {
                _frame.Navigate(typeof(TransparentPage));
            }
        }

        private async void PrimaryButton_Click(object sender, RoutedEventArgs e)
        {
            IsActive = true;
            ShowError = false;
            if (string.IsNullOrWhiteSpace(ChannelName))
            {
                ErrorText = "群组名称不能为空";
                ShowError = true;
                ChannelNameTextBox.Focus(FocusState.Programmatic);
            }
            else
            {
                var avatars = Avatars.Select(a => a.Id);
                string uids = string.Join(',', avatars);
                var visibility = IsPrivate ? WtVisibility.Private : WtVisibility.Public;
                var data = await _messageService.CreateChannelAsync(uids, ChannelName.Trim(), Color, Description.Trim(), visibility);
                if (data.Code == 200)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        await Task.Delay(20);
                        var firstSession = _masterPage.Sessions.First();
                        if (firstSession.Id == data.Data.Id && _masterPage.SelectedSession != firstSession)
                        {
                            _masterPage.SelectedSession = firstSession;
                            break;
                        }
                    }
                }
                else if (data.Code == 3004)
                {
                    ErrorText = "已有同名群组";
                    ShowError = true;
                    ChannelNameTextBox.Focus(FocusState.Programmatic);
                }
                else
                {
                    ErrorText = "创建失败";
                    ShowError = true;
                }
            }
            IsActive = false;
        }
    }
}
