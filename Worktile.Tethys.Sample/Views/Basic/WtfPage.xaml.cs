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
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Worktile.Tethys.Sample.Models;

namespace Worktile.Tethys.Sample.Views.Basic
{
    public sealed partial class WtfPage : Page, INotifyPropertyChanged
    {
        public WtfPage()
        {
            InitializeComponent();
            IsActive = true;
            Wtfs = new ObservableCollection<Wtf>();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        ObservableCollection<Wtf> Wtfs { get; }

        bool _isActive;
        bool IsActive
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

        private async Task AddWtfItemsAsync()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                int begin = Convert.ToInt32("e600", 16);
                int end = Convert.ToInt32("e73C", 16);
                for (int i = begin; i <= end; i++)
                {
                    //if (i < 59155 || i > 59158)
                    //{
                        Wtfs.Add(new Wtf
                        {
                            Glyph = (char)i,
                            Unicode = "u+" + i.ToString("x")
                        });
                    //}
                }
            });
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await AddWtfItemsAsync();
            IsActive = false;
        }
    }
}
