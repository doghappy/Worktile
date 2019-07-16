using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Worktile.Common;

namespace Worktile.Controls
{
    public sealed partial class WtColorPicker : UserControl, INotifyPropertyChanged
    {
        public WtColorPicker()
        {
            InitializeComponent();
            Colors = WtColorHelper.Map.Select(m => m.NewColor).ToList();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public List<string> Colors { get; }

        private string _selectedColor;
        public string SelectedColor
        {
            get => _selectedColor;
            set
            {
                if (_selectedColor != value)
                {
                    _selectedColor = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedColor)));
                }
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            SelectedColor = Colors.First();
        }

        private void GridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var gridView = sender as GridView;
            if (e.AddedItems.Any())
            {
                if (gridView.ContainerFromItem(e.AddedItems[0]) is GridViewItem selectedItem)
                {
                    var fontIcon = selectedItem.GetChild<FontIcon>("CheckIcon");
                    fontIcon.Visibility = Visibility.Visible;
                }
            }
            if (e.RemovedItems.Any())
            {
                if (gridView.ContainerFromItem(e.RemovedItems[0]) is GridViewItem removedItem)
                {
                    var fontIcon = removedItem.GetChild<FontIcon>("CheckIcon");
                    fontIcon.Visibility = Visibility.Collapsed;
                }
            }
        }
    }
}
