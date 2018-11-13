﻿using System;
using System.Collections.Generic;
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
using System.ComponentModel;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Worktile.ApiModel.ApiMissionVnextWorkAddon;
using Worktile.Services;
using System.Threading.Tasks;
using Worktile.WtRequestClient;
using Worktile.ApiModel.ApiMissionVnextWorkAnalyticInsightGroups;
using System.Collections.ObjectModel;

namespace Worktile.Views.Mission.AnalyticInsight
{
    public sealed partial class IndexPage : Page, INotifyPropertyChanged
    {
        public IndexPage()
        {
            InitializeComponent();
            TopNavItems = new ObservableCollection<TopNavItem>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private ApiModel.ApiMissionVnextWorkAddon.Value _navItem;

        public ObservableCollection<TopNavItem> TopNavItems { get; }

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
                    //FrameNavigate();
                }
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            _navItem = e.Parameter as ApiModel.ApiMissionVnextWorkAddon.Value;
            TopNavIcon = WtIconHelper.GetGlyph(_navItem.Icon);
            TopNavName = _navItem.Name;
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await RequestApiMissionVnextWorkAnalyticInsightGroups();
        }

        private async Task RequestApiMissionVnextWorkAnalyticInsightGroups()
        {
            string uri = "/api/mission-vnext/work/analytic-insight/groups";
            var client = new WtHttpClient();
            var data = await client.GetAsync<ApiMissionVnextWorkAnalyticInsightGroups>(uri);
            foreach (var item in data.Data.Value)
            {
                var tni = new TopNavItem
                {
                    Id = item.Id,
                    Name = item.Name,
                    SubItems = new List<TopNavItem>()
                };
                foreach (var ins in item.Insights)
                {
                    tni.SubItems.Add(new TopNavItem
                    {
                        Id = ins.Id,
                        Name = ins.Name
                    });
                }
                TopNavItems.Add(tni);
            }
            SelectedNav = TopNavItems.First();
        }
    }

    public class TopNavItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<TopNavItem> SubItems { get; set; }
    }
}
