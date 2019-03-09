using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Worktile.ApiModels.ApiMissionVnextTask;
using Worktile.Common;
using Worktile.Domain.Mission.Info;

namespace Worktile.Views.Mission.Project.Detail
{
    public sealed partial class InfoPage : Page
    {
        public InfoPage()
        {
            InitializeComponent();
            Properties = new ObservableCollection<PropertyItem>();
        }

        private Data _task;

        ObservableCollection<PropertyItem> Properties { get; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            _task = e.Parameter as Data;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
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

            reader.LoadOptions(_task.Value.Id, Properties, Dispatcher);
        }
    }

    class PropertyItem : INotifyPropertyChanged
    {
        public PropertyItem()
        {
            DataSource = new ObservableCollection<DropdownItem>();
            SelectedIds = new List<string>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public string Header { get; set; }

        /// <summary>
        /// 用来获取Options
        /// </summary>
        public string PropertyId { get; set; }
        public string Value { get; set; }
        public string Glyph { get; set; }
        public SolidColorBrush Color { get; set; }
        public bool IsReadonly { get; set; }
        public ObservableCollection<DropdownItem> DataSource { get; }
        public string Control { get; set; }
        public DateTime? Date { get; set; }
        public TimeSpan TimeSpan { get; set; }
        public DateTime? Begin { get; set; }
        public DateTime? End { get; set; }

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

        public List<string> SelectedIds { get; }

        private List<DropdownItem> _selectedValues;
        public List<DropdownItem> SelectedValues
        {
            get => _selectedValues;
            set
            {
                if (_selectedValues != value)
                {
                    _selectedValues = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedValues)));
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
