using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace Worktile.Tethys
{
    public sealed class MultipleComboBox : ListView
    {
        public MultipleComboBox()
        {
            DefaultStyleKey = typeof(MultipleComboBox);
            base.SelectionMode = ListViewSelectionMode.Multiple;
        }

        //private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{

        //}

        //public new event SelectionChangedEventHandler SelectionChanged;

        private new ListViewSelectionMode SelectionMode => ListViewSelectionMode.Multiple;

        public DataTemplate SelectedItemTemplate
        {
            get { return (DataTemplate)GetValue(SelectedItemTemplateProperty); }
            set { SetValue(SelectedItemTemplateProperty, value); }
        }

        public static readonly DependencyProperty SelectedItemTemplateProperty =
            DependencyProperty.Register("SelectedItemTemplate", typeof(DataTemplate), typeof(MultipleComboBox), new PropertyMetadata(null));
        
    }
}
