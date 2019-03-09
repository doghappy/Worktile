using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Worktile.ApiModels.ApiMissionVnextKanbanContent;
using Worktile.Common;
using Worktile.Domain.Mission.Kanban;
using Worktile.Models.Kanban;
using Worktile.Common.WtRequestClient;

namespace Worktile.Views.Mission.Project
{
    public sealed partial class KanbanPage : Page, INotifyPropertyChanged
    {
        public KanbanPage()
        {
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private string _addonId;
        private string _viewId;
        private string _taskIdentifierPrefix;

        private List<KanbanGroup> _kanbanGroups;
        public List<KanbanGroup> KanbanGroups
        {
            get => _kanbanGroups;
            set
            {
                if (_kanbanGroups != value)
                {
                    _kanbanGroups = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(KanbanGroups)));
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
            await RequestContentAsync();
            IsActive = false;
        }

        private async Task RequestContentAsync()
        {
            string uri = $"/api/mission-vnext/kanban/{_addonId}/views/{_viewId}/content";
            var data = await WtHttpClient.GetAsync<ApiMissionVnextKanbanContent>(uri);

            var reader = new KanbanDataReader();
            KanbanGroups = reader.GetData(data);
        }

        private async void MyGrid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            await KanbanPageHelper.KanbanGridAdaptiveAsync(MyGrid);
        }
    }
}