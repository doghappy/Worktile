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
using Worktile.ApiModel.ApiMissionVnextWorkAddon;
using Worktile.Services;

namespace Worktile.Views.Mission
{
    public sealed partial class MyDirectedPage : Page, INotifyPropertyChanged
    {
        public MyDirectedPage()
        {
            InitializeComponent();
            TopNavItems = new ObservableCollection<TopNavItem>();
            Status = new List<string> { "活动任务", "完成任务" };
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private Value _navItem;

        public ObservableCollection<TopNavItem> TopNavItems { get; }

        public List<string> Status { get; }

        private string _selectedStatus;
        public string SelectedStatus
        {
            get => _selectedStatus;
            set
            {
                if (SelectedStatus != value)
                {
                    _selectedStatus = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedStatus)));
                }
            }
        }

        private string _topNavIcon;
        public string TopNavIcon
        {
            get => _topNavIcon;
            set
            {
                _topNavIcon = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TopNavIcon)));
            }
        }

        private string _topNavName;
        public string TopNavName
        {
            get => _topNavName;
            set
            {
                _topNavName = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TopNavName)));
            }
        }

        private TopNavItem _selectedNav;
        public TopNavItem SelectedNav
        {
            get => _selectedNav;
            set
            {
                if (SelectedNav != value)
                {
                    _selectedNav = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedNav)));
                }
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            _navItem = e.Parameter as Value;
            TopNavIcon = WtfIconHelper.GetGlyph(_navItem.Icon);
            TopNavName = _navItem.Name;
            SelectedStatus = Status.First();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (var item in _navItem.Features)
            {
                TopNavItems.Add(new TopNavItem
                {
                    Key = item.Key,
                    Name = item.Name
                });
            }
            SelectedNav = TopNavItems.First();
        }
    }

    public class TopNavItem
    {
        public string Key { get; set; }
        public string Name { get; set; }
    }
}
