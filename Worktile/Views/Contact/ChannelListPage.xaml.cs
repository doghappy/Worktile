using System.Collections.ObjectModel;
using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Worktile.Common.Extensions;
using Worktile.Models;
using Worktile.Models.Message.Session;
using Worktile.Services;

namespace Worktile.Views.Contact
{
    public sealed partial class ChannelListPage : Page, INotifyPropertyChanged
    {
        public ChannelListPage()
        {
            InitializeComponent();
            Sessions = new ObservableCollection<ChannelSession>();
            _messageService = new MessageService();
        }

        readonly MessageService _messageService;
        public event PropertyChangedEventHandler PropertyChanged;

        ObservableCollection<ChannelSession> Sessions { get; }

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
            var channels = await _messageService.GetAllChannelsAsync();
            foreach (var item in channels)
            {
                item.ForShowAvatar();
                Sessions.Add(item);
            }
            IsActive = false;
        }
    }
}
