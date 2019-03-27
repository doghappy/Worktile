using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Worktile.Common.Extensions;
using Worktile.Models.Department;
using Worktile.Services;

namespace Worktile.Views.Contact
{
    public sealed partial class OrganizationStructurePage : Page
    {
        public OrganizationStructurePage()
        {
            InitializeComponent();
            _teamService = new TeamService();
            DepartmentNodes = new ObservableCollection<DepartmentNode>();
        }

        readonly TeamService _teamService;

        ObservableCollection<DepartmentNode> DepartmentNodes { get; }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var data = await _teamService.GetDepartmentsTreeAsync();
            foreach (var item in data)
            {
                item.ForShowAvatar();
                DepartmentNodes.Add(item);
            }
        }
    }
}
