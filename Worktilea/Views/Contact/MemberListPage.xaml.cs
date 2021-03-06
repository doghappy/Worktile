﻿using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Worktile.Common;
using Worktile.Common.Extensions;
using Worktile.Models;
using Worktile.Views.Contact.Detail;

namespace Worktile.Views.Contact
{
    public sealed partial class MemberListPage : Page
    {
        public MemberListPage()
        {
            InitializeComponent();
            Avatars = new ObservableCollection<GroupWrapper>();
        }

        ObservableCollection<GroupWrapper> Avatars { get; }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var avatars = DataSource.Team.Members
                 .Where(m => m.IsTrueMember())
                 .Select(m => m.TethysAvatar);
            var group = avatars
                .GroupBy(a => a.DisplayNamePinyin[0].ToString().ToUpper())
                .Select(a => new GroupWrapper(a.OrderBy(i => i.DisplayNamePinyin))
                {
                    Key = a.Key
                })
                .OrderBy(g => g.Key);
            foreach (var item in group)
            {
                Avatars.Add(item);
            }
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var avatar = e.AddedItems[0] as TethysAvatar;
            var masterPage = this.GetParent<MasterPage>();
            masterPage.ContentFrameNavigate(typeof(MemberDetailPage), avatar);
        }
    }
}
