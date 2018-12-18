using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Worktile.ApiModel.ApiMissionVnextTask;
using Worktile.Common;
using Worktile.Domain.Mission.Info;
using Worktile.Enums;
using Worktile.Models.Mission;
using Worktile.WtRequestClient;

namespace Worktile.Views.Mission.Project.Detail
{
    public sealed partial class InfoPage : Page, INotifyPropertyChanged
    {
        public InfoPage()
        {
            InitializeComponent();
            Properties = new ObservableCollection<PropertyItem>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private Data _task;

        ObservableCollection<PropertyItem> Properties { get; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            _task = e.Parameter as Data;
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Properties.Add(new PropertyItem
            {
                Header = "项目名称",
                Value = _task.References.Project.Name,
                Glyph = WtIconHelper.GetGlyph(_task.References.Project.Visibility),
                IsReadonly = true,
                Color = new SolidColorBrush((Color)Resources["SystemAccentColor"])
            });

            string taskTypeId = _task.Value.TaskTypeId;
            var taskType = _task.References.TaskTypes.Single(t => t.Id == taskTypeId);
            Properties.Add(new PropertyItem
            {
                Header = "任务类型",
                Value = taskType.Name,
                Glyph = WtIconHelper.GetGlyph("wtf-type-" + taskType.Icon),
                IsReadonly = true,
                Color = WtColorHelper.GetSolidColorBrush(WtColorHelper.GetColorByClass(taskType.Icon))
            });

            var reader = new PropertiesReader();
            var props = reader.Read(_task);
            foreach (var item in props)
            {
                Properties.Add(item);
            }
            await LoadOptionsAsync();
        }

        private async Task LoadOptionsAsync()
        {
            var reader = new PropertiesReader();
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                async () => await reader.LoadOptionsAsync(_task.Value.Id, Properties));
        }
    }

    class PropertyItem : INotifyPropertyChanged
    {
        public PropertyItem()
        {
            DataSource = new ObservableCollection<DropdownItem>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public string Header { get; set; }

        /// <summary>
        /// 当数据类型是下拉框时，此值表示PropertyId，可用来获取Options。
        /// </summary>
        public string Value { get; set; }
        public string Glyph { get; set; }
        public SolidColorBrush Color { get; set; }
        public bool IsReadonly { get; set; }
        public ObservableCollection<DropdownItem> DataSource { get; set; }
        public string Control { get; set; }

        private DropdownItem _selectedValue;
        public DropdownItem SelectedValue
        {
            get => _selectedValue;
            set
            {
                if (_selectedValue != value)
                {
                    _selectedValue = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedValue)));
                }
            }
        }
    }

    class DropdownItem
    {
        public string Glyph { get; set; }
        public SolidColorBrush Color { get; set; }
        public string Text { get; set; }
        public string Value { get; set; }
    }
}
