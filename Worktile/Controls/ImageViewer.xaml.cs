using System.Collections.Generic;
using System.ComponentModel;
using Windows.UI.Xaml.Controls;
using Worktile.Message.Models;

namespace Worktile.Controls
{
    public sealed partial class ImageViewer : UserControl, INotifyPropertyChanged
    {
        public ImageViewer()
        {
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private object _images;
        public object Images
        {
            get => _images;
            set
            {
                if (_images != value)
                {
                    _images = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Images)));
                }
            }
        }

    }
}
