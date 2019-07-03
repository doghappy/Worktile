using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Worktile.Common;

namespace Worktile.Controls
{
    public sealed partial class WtColorPicker : UserControl
    {
        public WtColorPicker()
        {
            InitializeComponent();
            Colors = WtColorHelper.Map.Select(m => m.NewColor).ToList();
        }

        public List<string> Colors { get; }

        public string SelectedColor
        {
            get { return (string)GetValue(SelectedColorProperty); }
            set { SetValue(SelectedColorProperty, value); }
        }
        public static readonly DependencyProperty SelectedColorProperty =
            DependencyProperty.Register("SelectedColor", typeof(string), typeof(WtColorPicker), new PropertyMetadata(null));

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            SelectedColor = WtColorHelper.Map.First().NewColor;
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
