using Microsoft.Toolkit.Uwp.Helpers;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Worktile.Common;
using Worktile.Common.Communication;
using Worktile.Enums;
using Worktile.Enums.Message;
using Worktile.Models;
using Worktile.Models.Message;
using Worktile.Models.Message.NavigationParam;
using Worktile.Models.Message.Session;
using Worktile.Services;
using Worktile.ViewModels.Message;
using Worktile.ViewModels.Message.Detail.Content;

namespace Worktile.Views.Message.Detail.Content
{
    public sealed partial class AssistantMessagePage : Page, INotifyPropertyChanged
    {
        public AssistantMessagePage()
        {
            InitializeComponent();
            _assistantMessageService = new AssistantMessageService();
            Messages = new ObservableCollection<Models.Message.Message>();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        readonly AssistantMessageService _assistantMessageService;
        private MemberSession _session;
        private TopNav _nav;

        public ObservableCollection<Models.Message.Message> Messages { get; }

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

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            IsActive = true;

            var detailPage = this.GetParent<AssistantDetailPage>();
            _session = detailPage.Session;
            _nav = detailPage.SelectedNav;

            await LoadMessagesAsync();
            await ClearUnReadAsync();

            WtSocket.OnMessageReceived += OnMessageReceived;
            IsActive = false;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            WtSocket.OnMessageReceived -= OnMessageReceived;
        }

        private async void ScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            var scrollViewer = sender as ScrollViewer;
            if (_assistantMessageService.HasMore.HasValue
                && _assistantMessageService.HasMore.Value
                && !IsActive && scrollViewer.VerticalOffset <= 10)
            {
                IsActive = true;
                await LoadMessagesAsync();
                IsActive = false;
            }
        }

        private async void Pin_Click(object sender, RoutedEventArgs e)
        {
            var flyoutItem = sender as MenuFlyoutItem;
            var msg = flyoutItem.DataContext as Models.Message.Message;
            bool result = await _assistantMessageService.PinAsync(msg.Id, _session.Id);
            if (result)
            {
                msg.IsPinned = true;
            }
        }

        private async void UnPin_Click(object sender, RoutedEventArgs e)
        {
            var flyoutItem = sender as MenuFlyoutItem;
            var msg = flyoutItem.DataContext as Models.Message.Message;
            bool result = await _assistantMessageService.UnPinAsync(msg.Id, _session.Id);
            if (result)
            {
                msg.IsPinned = false;
            }
        }

        private async Task LoadMessagesAsync()
        {
            var messages = await _assistantMessageService.LoadMessagesAsync(_session, _nav.FilterType);
            foreach (var item in messages)
            {
                Messages.Insert(0, item);
            }
        }

        private async Task ClearUnReadAsync()
        {
            bool result = await _assistantMessageService.ClearUnReadAsync(_session.Id);
            if (result)
            {
                App.UnreadBadge -= _session.UnRead;
                await Task.Run(async () => await DispatcherHelper.ExecuteOnUIThreadAsync(() => _session.UnRead = 0));
            }
        }

        public async void OnMessageReceived(Models.Message.Message message)
        {
            if (message.To.Id == _session.Id)
            {
                await Task.Run(async () =>
                {
                    await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
                    {
                        var member = DataSource.Team.Members.Single(m => m.Uid == message.From.Uid);
                        message.From.TethysAvatar = new TethysAvatar
                        {
                            DisplayName = member.DisplayName,
                            Source = AvatarHelper.GetAvatarBitmap(member.Avatar, AvatarSize.X80, FromType.User),
                            Background = AvatarHelper.GetColorBrush(member.DisplayName)
                        };
                        message.IsPinned = false;
                        Messages.Add(message);
                    });
                });
            }
        }
    }
}
