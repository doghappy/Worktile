using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Worktile.Common;
using Worktile.Models.Member;

namespace Worktile.Views.Contact
{
    public sealed partial class MasterPage : Page
    {
        public MasterPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            MembersFrame.Navigate(typeof(MemberListPage));
        }

        private async void AddMember_Click(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri(DataSource.SubDomain + "/console/members?add=true"));
        }
        
        private void OrgStructure_Click(object sender, RoutedEventArgs e)
        {
            if (MembersFrame.CurrentSourcePageType != typeof(OrganizationStructurePage))
            {
                MembersFrame.Navigate(typeof(OrganizationStructurePage));
            }
        }

        private void Contacts_Click(object sender, RoutedEventArgs e)
        {
            if (MembersFrame.CurrentSourcePageType != typeof(MemberListPage))
            {
                MembersFrame.Navigate(typeof(MemberListPage));
            }
        }

        private void Channels_Click(object sender, RoutedEventArgs e)
        {
            if (MembersFrame.CurrentSourcePageType != typeof(ChannelListPage))
            {
                MembersFrame.Navigate(typeof(ChannelListPage));
            }
        }

        private void Robots_Click(object sender, RoutedEventArgs e)
        {
            if (MembersFrame.CurrentSourcePageType != typeof(RobotListPage))
            {
                MembersFrame.Navigate(typeof(RobotListPage));
            }
        }
    }
}
