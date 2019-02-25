using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Worktile.ApiModels;
using Worktile.ApiModels.ApiTeamChats;
using Worktile.Common;
using Worktile.Common.WtRequestClient;

namespace Worktile.Views.Message.Dialog
{
    public sealed partial class JoinGroupDialog : ContentDialog, INotifyPropertyChanged
    {
        public JoinGroupDialog()
        {
            InitializeComponent();
            Sessions = new ObservableCollection<Session>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<Session> Sessions { get; }

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

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private async void ContentDialog_Loaded(object sender, RoutedEventArgs e)
        {
            IsActive = true;
            var client = new WtHttpClient();
            string url = "/api/channels?type=all&filter=all&status=ok";
            var data = await client.GetAsync<ApiDataResponse<List<Channel>>>(url);
            foreach (var item in data.Data)
            {
               Sessions.Add(MessageHelper.GetSession(item));
            }
            IsActive = false;
        }
    }
}
