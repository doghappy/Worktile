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
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Worktile.ApiModel.ApiMissionVnextTableContent;
using Worktile.Common;
using Worktile.Enums;
using Worktile.Models.Mission.WtTask;
using Worktile.WtRequestClient;

namespace Worktile.Views.Mission.Project
{
    public sealed partial class TablePage : Page, INotifyPropertyChanged
    {
        public TablePage()
        {
            InitializeComponent();
            HeaderItems = new ObservableCollection<HeaderItem>();
        }

        private string _addonId;
        private string _viewId;
        private string _taskIdentifierPrefix;

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<HeaderItem> HeaderItems { get; }

        private bool _isActive;
        public bool IsActive
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

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            dynamic parameters = e.Parameter;
            _addonId = parameters.AddonId;
            _viewId = parameters.ViewId;
            _taskIdentifierPrefix = parameters.TaskIdentifierPrefix;
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            IsActive = true;
            await RequestDataAsync();
            IsActive = false;
        }

        private async Task RequestDataAsync()
        {
            //string uri = $"/api/mission-vnext/table/{_addonId}/views/{_viewId}/content?pi=0&ps=20";
            string uri = $"/api/mission-vnext/table/{_addonId}/views/{_viewId}/content";
            var client = new WtHttpClient();
            var data = await client.GetAsync<ApiMissionVnextTableContent>(uri);
            ReadHeaderItems(data);
        }

        private void ReadHeaderItems(ApiMissionVnextTableContent data)
        {
            foreach (var item in data.Data.References.Columns)
            {
                var property = data.Data.References.Properties.Single(p => p.Id == item);
                var headerItem = new HeaderItem
                {
                    Text = property.Name
                };
                switch (property.Type)
                {
                    case WtTaskPropertyType.Number:
                        headerItem.Width = 100;
                        break;
                    case WtTaskPropertyType.Text:
                    case WtTaskPropertyType.DropDown:
                    case WtTaskPropertyType.Member:
                    case WtTaskPropertyType.Iteration:
                    case WtTaskPropertyType.Priority:
                    case WtTaskPropertyType.TaskType:
                    case WtTaskPropertyType.File:
                        headerItem.Width = 160;
                        break;
                    case WtTaskPropertyType.State:
                        headerItem.Width = 180;
                        break;
                    case WtTaskPropertyType.DateTime:
                    case WtTaskPropertyType.DateSpan:
                    case WtTaskPropertyType.MultiMember:
                    case WtTaskPropertyType.Workload:
                    case WtTaskPropertyType.Tag:
                    case WtTaskPropertyType.MultiSelect:
                        headerItem.Width = 200;
                        break;
                    case WtTaskPropertyType.MultiText:
                        headerItem.Width = 400;
                        break;
                }
                HeaderItems.Add(headerItem);
            }
        }
    }

    public class HeaderItem
    {
        public string Text { get; set; }
        public double Width { get; set; }
    }
}
