using System;
using System.Collections.Generic;
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
using Worktile.Models;
using Worktile.ViewModels.Message;
using Worktile.ViewModels.Message.Detail;

namespace Worktile.Views.Message.Detail
{
    public sealed partial class CreateChatPage : Page
    {
        public CreateChatPage()
        {
            InitializeComponent();
        }

        CreateChatViewModel ViewModel { get; set; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var masterViewModel = e.Parameter as MasterViewModel;
            ViewModel = new CreateChatViewModel(masterViewModel);
        }

        private async void SendMessageButton_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var avatar = btn.DataContext as TethysAvatar;
            await ViewModel.CreateAsync(avatar);
        }
    }
}
