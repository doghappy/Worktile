using System;
using System.Collections.Generic;
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

namespace Worktile.WtControls.Mission
{
    public sealed partial class MissionListViewHeader : UserControl, INotifyPropertyChanged
    {
        public MissionListViewHeader()
        {
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private string _header;
        public string Header
        {
            get => _header;
            set
            {
                _header = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Header)));
            }
        }

        private int _notStarted;
        public int NotStarted
        {
            get => _notStarted;
            set
            {
                _notStarted = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NotStarted)));
            }
        }

        private int _processing;
        public int Processing
        {
            get => _processing;
            set
            {
                _processing = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Processing)));
            }
        }

        private int _completed;
        public int Completed
        {
            get => _completed;
            set
            {
                _completed = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Completed)));
            }
        }
    }
}
