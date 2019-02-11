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
using Worktile.Models.IM;
using Worktile.ViewModels.IM;
using Worktile.Common;

namespace Worktile.Views.IM
{
    public sealed partial class SessionMessagePage : Page
    {
        public SessionMessagePage()
        {
            InitializeComponent();
            ViewModel = new SessionMessageViewModel();
        }

        SessionMessageViewModel ViewModel { get; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ViewModel.Session = e.Parameter as ChatSession;
        }
    }
}
