using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Worktile
{
    public sealed partial class TestPage : Page, INotifyPropertyChanged
    {
        public TestPage()
        {
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private string _emoji;
        public string Emoji
        {
            get => _emoji;
            set
            {
                if (_emoji != value)
                {
                    _emoji = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Emoji)));
                }
            }
        }

        private void EmojiPicker_OnEmojiSelected(string emoji)
        {
            Emoji = emoji;
        }

        private void Flyout_Opened(object sender, object e)
        {
            var flyout = sender as Flyout;
        }
    }
}