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

namespace Worktile.Message
{
    public sealed partial class MessageDetailPage : Page
    {
        public MessageDetailPage()
        {
            InitializeComponent();
            ViewModel = new MessageDetailViewModel();
        }

        public MessageDetailViewModel ViewModel { get; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ViewModel.Session = e.Parameter as Session;
            ViewModel.LoadNavs();
        }
    }
}
