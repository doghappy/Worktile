using System.Collections.ObjectModel;
using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Worktile.Common;
using Worktile.Common.Extensions;
using Worktile.Enums;
using Worktile.Models.Department;
using Worktile.Services;
using Worktile.Views.Contact.Detail;

namespace Worktile.Views.Contact
{
    public sealed partial class OrganizationStructurePage : Page, INotifyPropertyChanged
    {
        public OrganizationStructurePage()
        {
            InitializeComponent();
            _teamService = new TeamService();
            DepartmentNodes = new ObservableCollection<DepartmentNode>();
        }

        readonly TeamService _teamService;
        public event PropertyChangedEventHandler PropertyChanged;

        ObservableCollection<DepartmentNode> DepartmentNodes { get; }

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
            var data = await _teamService.GetDepartmentsTreeAsync();
            foreach (var item in data)
            {
                item.ForShowAvatar(AvatarSize.X160);
                DepartmentNodes.Add(item);
            }
            IsActive = false;
        }

        private void TreeView_ItemInvoked(TreeView sender, TreeViewItemInvokedEventArgs args)
        {
            var node = args.InvokedItem as DepartmentNode;
            if (node.Type == DepartmentNodeType.Member)
            {
                var masterPage = this.GetParent<MasterPage>();
                masterPage.ContentFrameNavigate(typeof(MemberDetailPage), node.Avatar);
            }
        }
    }
}
