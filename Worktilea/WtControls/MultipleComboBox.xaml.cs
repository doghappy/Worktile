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

namespace Worktile.WtControls
{
    public sealed partial class MultipleComboBox : UserControl, INotifyPropertyChanged
    {
        public MultipleComboBox()
        {
            InitializeComponent();
            _emptyTextVisibility = Visibility.Visible;
            _selectedVisibility = Visibility.Collapsed;
            _selectedItems = new ObservableCollection<object>();
        }

        private ObservableCollection<object> _selectedItems;

        public event PropertyChangedEventHandler PropertyChanged;

        public DataTemplate ListItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        public static readonly DependencyProperty ItemTemplateProperty =
            DependencyProperty.Register("ListItemTemplate", typeof(int), typeof(MultipleComboBox), new PropertyMetadata(null));

        public DataTemplate SelectedItemTemplate
        {
            get { return (DataTemplate)GetValue(SelectedItemTemplateProperty); }
            set { SetValue(SelectedItemTemplateProperty, value); }
        }

        public static readonly DependencyProperty SelectedItemTemplateProperty =
            DependencyProperty.Register("SelectedItemTemplate", typeof(DataTemplate), typeof(MultipleComboBox), new PropertyMetadata(null));

        public object ItemsSource
        {
            get { return GetValue(ItemSourceProperty); }
            set { SetValue(ItemSourceProperty, value); }
        }

        public static readonly DependencyProperty ItemSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(object), typeof(MultipleComboBox), new PropertyMetadata(null));

        public bool IsMultiSelectCheckBoxEnabled
        {
            get { return (bool)GetValue(IsMultiSelectCheckBoxEnabledProperty); }
            set { SetValue(IsMultiSelectCheckBoxEnabledProperty, value); }
        }

        public static readonly DependencyProperty IsMultiSelectCheckBoxEnabledProperty =
            DependencyProperty.Register("IsMultiSelectCheckBoxEnabled", typeof(bool), typeof(MultipleComboBox), new PropertyMetadata(true));

        public string EmptyText
        {
            get { return (string)GetValue(EmptyTextProperty); }
            set { SetValue(EmptyTextProperty, value); }
        }

        public static readonly DependencyProperty EmptyTextProperty =
            DependencyProperty.Register("EmptyText", typeof(string), typeof(MultipleComboBox), new PropertyMetadata(string.Empty));

        private Visibility _selectedVisibility;
        private Visibility SelectedVisibility
        {
            get => _selectedVisibility;
            set
            {
                if (_selectedVisibility != value)
                {
                    _selectedVisibility = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedVisibility)));
                }
            }
        }

        private Visibility _emptyTextVisibility;
        private Visibility EmptyTextVisibility
        {
            get => _emptyTextVisibility;
            set
            {
                if (_emptyTextVisibility != value)
                {
                    _emptyTextVisibility = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EmptyTextVisibility)));
                }
            }
        }

        //bool _flag = false;

        private object _initSelectedItems;
        public object InitSelectedItems
        {
            get => _initSelectedItems;
            set
            {
                if (_initSelectedItems != value && value != null)
                {
                    _initSelectedItems = value;
                    var val = value as IEnumerable<object>;
                    foreach (var item in val)
                    {
                        _selectedItems.Add(item);
                    }
                    if (_selectedItems.Any())
                    {
                        SelectedVisibility = Visibility.Visible;
                        EmptyTextVisibility = Visibility.Collapsed;

                    }
                }
            }
        }

        private void Flyout_Opened(object sender, object e)
        {

        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (var item in e.AddedItems)
            {
                if (!_selectedItems.Contains(item))
                {
                    _selectedItems.Add(item);
                }
            }
            foreach (var item in e.RemovedItems)
            {
                if (_selectedItems.Contains(item))
                {
                    _selectedItems.Remove(item);
                }
            }
            if (_selectedItems.Any())
            {
                SelectedVisibility = Visibility.Visible;
                EmptyTextVisibility = Visibility.Collapsed;
            }
            else
            {
                SelectedVisibility = Visibility.Collapsed;
                EmptyTextVisibility = Visibility.Visible;
            }
        }
    }
}
