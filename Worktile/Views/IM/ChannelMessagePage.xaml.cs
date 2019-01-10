using Microsoft.Toolkit.Collections;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Worktile.Common;
using Worktile.Models.IM;
using Worktile.Models.IM.Message;
using Worktile.ViewModels.IM;

namespace Worktile.Views.IM
{
    public sealed partial class ChannelMessagePage : Page
    {
        public ChannelMessagePage()
        {
            InitializeComponent();
            ViewModel = new ChannelMessageViewModel();
        }

        ChannelMessageViewModel ViewModel { get; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ViewModel.SelectedNav = ViewModel.NavItems.First();
            ViewModel.Session = e.Parameter as ChatSession;
        }

        private async void ScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            var scrollViewer = sender as ScrollViewer;
            if (ViewModel.SelectedNav.HasMore.HasValue && ViewModel.SelectedNav.HasMore.Value
                && !ViewModel.IsActive && scrollViewer.VerticalOffset <= 28)
            {
                await ViewModel.LoadMessagesAsync();
            }
        }

        private async void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!ViewModel.SelectedNav.HasMore.HasValue)
            {
                //当HasMore没有值时，表示没有载入过消息，需要请求接口。
                await ViewModel.LoadMessagesAsync();
            }
        }
    }
}
