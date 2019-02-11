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
            ViewModel.Session = e.Parameter as ChatSession;
        }
    }
}
