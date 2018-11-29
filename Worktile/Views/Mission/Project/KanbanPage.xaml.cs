using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Worktile.Common;
using Worktile.ViewModels.Mission.Project;

namespace Worktile.Views.Mission.Project
{
    public sealed partial class KanbanPage : Page
    {
        public KanbanPage()
        {
            InitializeComponent();
            KanBanGroups = new ObservableCollection<KanbanGroup>();
            _groupedIds = new List<string>();
            _notInStackProperties = new[] { "tag", "task_state_id", "title", "task_type_id", "updated_at", "created_at", "attachment", "assignee", "priority" };
        }

        readonly string[] _notInStackProperties;

        public event PropertyChangedEventHandler PropertyChanged;

        private List<string> _groupedIds;

        public ObservableCollection<KanbanGroup> KanBanGroups { get; }

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

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            IsActive = true;
            await RequestContentAsync();
            IsActive = false;
        }

        private async Task RequestContentAsync()
        {
            string uri = $"/api/mission-vnext/kanban/{AddonId}/views/{ViewId}/content";
            var client = new WtHttpClient();
            var data = await client.GetAsync<ApiMissionVnextKanbanContent>(uri);

            foreach (var item in data.Data.References.Groups)
            {
                var kbGroup = new KanbanGroup
                {
                    Header = item.Name
                };
                _groupedIds.AddRange(item.TaskIds);
                foreach (var taskId in item.TaskIds)
                {
                    ReadKanbanItem(data, kbGroup, taskId);
                }
                KanBanGroups.Add(kbGroup);
            }

        public KanbanViewModel ViewModel { get; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            dynamic parameters = e.Parameter;
            ViewModel.AddonId = parameters.AddonId;
            ViewModel.ViewId = parameters.ViewId;
            ViewModel.TaskIdentifierPrefix = parameters.TaskIdentifierPrefix;
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.IsActive = true;
            await ViewModel.RequestContentAsync();
            ViewModel.IsActive = false;
        }

        private async void MyGrid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            await KanbanPageHelper.KanbanGridAdaptiveAsync(MyGrid);
        }
    }
}
