using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Worktile.ApiModels;
using Worktile.ApiModels.ApiTeamChats;
using Worktile.Common.WtRequestClient;

namespace Worktile.Views.Message.Dialog
{
    public sealed partial class JoinGroupDialog : ContentDialog, INotifyPropertyChanged
    {
        public JoinGroupDialog()
        {
            InitializeComponent();
            Channels = new ObservableCollection<Channel>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<Channel> Channels { get; }

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

            }
            IsActive = false;
        }
    }
}
