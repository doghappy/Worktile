using System;
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
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Worktile.WindowsUI.Common;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace Worktile.WindowsUI.Views.Project
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class ProjectPage : Page
    {
        public ProjectPage()
        {
            this.InitializeComponent();
        }

        private void MasterDetailsView_Loaded(object sender, RoutedEventArgs e)
        {
            var workNode = new TreeViewNode
            {
                Content = "work"
            };

            var projectNode = new TreeViewNode
            {
                Content = "project"
            };
            var tree = Frame.GetChild<TreeView>("Tree");
            tree.RootNodes.Add(workNode);
            tree.RootNodes.Add(projectNode);
        }
    }
}
