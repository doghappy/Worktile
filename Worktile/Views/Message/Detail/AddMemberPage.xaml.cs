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
using Worktile.Common;
using Worktile.Common.Extensions;
using Worktile.Enums;
using Worktile.Models.Member;
using Worktile.Models.Message.Session;
using Worktile.ViewModels.Message.Detail;

namespace Worktile.Views.Message.Detail
{
    public sealed partial class AddMemberPage : Page
    {
        public AddMemberPage()
        {
            InitializeComponent();

        }

        AddMemberViewModel ViewModel { get; set; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var session = e.Parameter as ChannelSession;
            List<Member> members = new List<Member>();
            foreach (var item in DataSource.Team.Members)
            {
                if (item.Role != RoleType.Bot && session.Members.All(m => m.Uid != item.Uid))
                {
                    item.ForShowAvatar(AvatarSize.X80);
                    members.Add(item);
                }
            }
            ViewModel = new AddMemberViewModel(members);
        }
    }
}
