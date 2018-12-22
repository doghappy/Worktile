using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using Worktile.ApiModel.ApiMissionVnextTask;
using Worktile.Common;
using Worktile.WtRequestClient;

namespace Worktile.Views.Mission.Project.Detail
{
    public sealed partial class DetailPage : Page, INotifyPropertyChanged
    {
        public DetailPage()
        {
            InitializeComponent();
            _titleIconVisibility = Visibility.Collapsed;
            _titleGroupVisibility = Visibility.Visible;
            _titleTextBoxVisibility = Visibility.Collapsed;
            NavItems = new ObservableCollection<DetailNavItem>();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private Data _task;

        private string _taskId;
        public string TaskId
        {
            get => _taskId;
            set
            {
                if (_taskId != value)
                {
                    _taskId = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TaskId)));
                }
            }
        }

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

        private DetailNavItem _selectedNav;
        public DetailNavItem SelectedNav
        {
            get => _selectedNav;
            set
            {
                if (_selectedNav != value)
                {
                    _selectedNav = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedNav)));
                    ContentNavigate();
                }
            }
        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            //_taskId = e.Parameter.ToString();
            TaskId = "5b88ac1766014b61f79cefa1";
        }

        #region Task Title
        private Visibility _titleIconVisibility;
        public Visibility TitleIconVisibility
        {
            get => _titleIconVisibility;
            set
            {
                if (_titleIconVisibility != value)
                {
                    _titleIconVisibility = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TitleIconVisibility)));
                }
            }
        }

        private Visibility _titleGroupVisibility;
        public Visibility TitleGroupVisibility
        {
            get => _titleGroupVisibility;
            set
            {
                if (_titleGroupVisibility != value)
                {
                    _titleGroupVisibility = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TitleGroupVisibility)));
                }
            }
        }

        private Visibility _titleTextBoxVisibility;
        public Visibility TitleTextBoxVisibility
        {
            get => _titleTextBoxVisibility;
            set
            {
                if (_titleTextBoxVisibility != value)
                {
                    _titleTextBoxVisibility = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TitleTextBoxVisibility)));
                }
            }
        }


        private string _taskTitile;
        public string TaskTitle
        {
            get => _taskTitile;
            set
            {
                if (_taskTitile != value)
                {
                    _taskTitile = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TaskTitle)));
                }
            }
        }


        private void Title_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            TitleIconVisibility = Visibility.Visible;
        }

        private void Title_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            TitleIconVisibility = Visibility.Collapsed;
        }

        private void Title_Tapped(object sender, TappedRoutedEventArgs e)
        {
            TitleGroupVisibility = Visibility.Collapsed;
            TitleTextBoxVisibility = Visibility.Visible;
        }

        private void TaskTitle_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (!string.IsNullOrWhiteSpace(textBox.Text))
            {
                TitleGroupVisibility = Visibility.Visible;
                TitleTextBoxVisibility = Visibility.Collapsed;

                if (TaskTitle != textBox.Text)
                {
                    //请求修改

                    //修改成功后更新值
                    TaskTitle = textBox.Text;
                }
            }
        }
        #endregion

        #region Task State
        private Models.TaskState _state;
        public Models.TaskState State
        {
            get => _state;
            set
            {
                if (_state != value)
                {
                    _state = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(State)));
                }
            }
        }

        private List<Models.TaskState> _allStates;
        public List<Models.TaskState> AllStates
        {
            get => _allStates;
            set
            {
                if (_allStates != value)
                {
                    _allStates = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AllStates)));
                }
            }
        }
        #endregion

        public ObservableCollection<DetailNavItem> NavItems { get; }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            TestHelper.SignIn();

            IsActive = true;
            string uri = $"/api/mission-vnext/tasks/{TaskId}";
            var client = new WtHttpClient();
            var data = await client.GetAsync<ApiMissionVnextTask>(uri);
            _task = data.Data;
            TaskTitle = _task.Value.Title;

            var taskState = _task.References.Lookups.TaskStates.Single(t => t.Id == _task.Value.TaskStateId);
            State = new Models.TaskState
            {
                Id = taskState.Id,
                Foreground = WtColorHelper.GetNewColor(taskState.Color),
                BoldGlyph = WtIconHelper.GetLightGlyph(taskState.Type),
                LightGlyph = WtIconHelper.GetLightGlyph(taskState.Type),
                Name = taskState.Name
            };

            AddNavItems();
            IsActive = false;
        }

        private void AddNavItems()
        {
            NavItems.Add(new DetailNavItem
            {
                Text = "任务信息",
                Glyph = "\ue63c",
            });
            foreach (var item in _task.Value.Relations)
            {
                NavItems.Add(new DetailNavItem
                {
                    Text = item.Name,
                    Glyph = WtIconHelper.GetGlyphByRelationType(item.Type)
                });
            }
            NavItems.Add(new DetailNavItem
            {
                Text = "工时",
                Glyph = "\ue6fc",
            });
            NavItems.Add(new DetailNavItem
            {
                Text = "附件",
                Glyph = "\ue630",
            });
            SelectedNav = NavItems.First();
        }

        private async void Flyout_Opened(object sender, object e)
        {
            await StateList.OpenAsync();
        }

        private void ContentNavigate()
        {
            switch (SelectedNav.Text)
            {
                case "任务信息":
                    ContentFrame.Navigate(typeof(InfoPage), _task);
                    break;
            }
        }
    }

    public class DetailNavItem
    {
        public string Text { get; set; }
        public string Glyph { get; set; }
    }
}
