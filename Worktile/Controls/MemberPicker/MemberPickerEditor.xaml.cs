using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Worktile.ApiModels;
using Worktile.Common;
using Worktile.Common.WtRequestClient;
using Worktile.Enums;
using Worktile.Enums.Message;
using Worktile.Models;
using Worktile.Models.Department;
using Worktile.Views.Message;

namespace Worktile.Controls
{
    public sealed partial class MemberPickerEditor : UserControl//, INotifyPropertyChanged
    {
        public MemberPickerEditor()
        {
            InitializeComponent();
            UnSelectedAvatars = new ObservableCollection<TethysAvatar>();
            SelectedAvatars = new ObservableCollection<TethysAvatar>();
            //SelectedAvatars.CollectionChanged += SelectedAvatars_CollectionChanged;
            SuggestAvatars = new ObservableCollection<TethysAvatar>();
            DepartmentNodes = new ObservableCollection<DepartmentNode>();
        }

        public event Action<MemberPickerEditor> OnPrimaryButtonClick;
        public event Action<MemberPickerEditor> OnCloseButtonClick;

        //public event PropertyChangedEventHandler PropertyChanged;

        //private string _title;
        //public string Title
        //{
        //    get => _title;
        //    set
        //    {
        //        if (_title != value)
        //        {
        //            _title = value;
        //            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Title)));
        //        }
        //    }
        //}
        private List<DepartmentNode> _departmentNodeList { get; set; }

        public string Title { get; set; }

        public ObservableCollection<TethysAvatar> UnSelectedAvatars { get; }
        public ObservableCollection<TethysAvatar> SelectedAvatars { get; }
        public ObservableCollection<TethysAvatar> SuggestAvatars { get; }
        public ObservableCollection<DepartmentNode> DepartmentNodes { get; }

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var item = e.ClickedItem as TethysAvatar;
            UnSelectedAvatars.Remove(item);
            SelectedAvatars.Add(item);
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as AppBarButton;
            var item = btn.DataContext as TethysAvatar;
            UnSelectedAvatars.Add(item);
            SelectedAvatars.Remove(item);
        }

        private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                string text = sender.Text.Trim();
                if (text != string.Empty)
                {
                    SuggestAvatars.Clear();
                    var items = UnSelectedAvatars.Where(a => a.DisplayName.Contains(text) || a.DisplayNamePinyin.Any(p => p.Contains(text, StringComparison.CurrentCultureIgnoreCase)));
                    foreach (var item in items)
                    {
                        SuggestAvatars.Add(item);
                    }
                }
            }
        }

        private void AutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (args.ChosenSuggestion is TethysAvatar item)
            {
                SelectedAvatars.Add(item);
                UnSelectedAvatars.Remove(item);
                SuggestAvatars.Remove(item);
            }
        }

        private void AddMembers_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in UnSelectedAvatars)
            {
                SelectedAvatars.Add(item);
            }
            UnSelectedAvatars.Clear();
            SuggestAvatars.Clear();
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in SelectedAvatars)
            {
                UnSelectedAvatars.Add(item);
            }
            SelectedAvatars.Clear();
        }

        // 加载部门树，TreeView控件存在Bug
        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (!UnSelectedAvatars.Any())
            {
                foreach (var item in DataSource.Team.Members)
                {
                    if (item.Role != RoleType.Bot)
                    {
                        UnSelectedAvatars.Add(AvatarHelper.GetAvatar(item, AvatarSize.X40));
                    }
                }
            }

            if (!DepartmentNodes.Any())
            {
                string url = $"/api/departments/tree?async=false";
                var data = await WtHttpClient.GetAsync<ApiDataResponse<List<DepartmentNode>>>(url);
                _departmentNodeList = new List<DepartmentNode>();
                foreach (var item in data.Data)
                {
                    SetAvatar(item);
                    DepartmentNodes.Add(item);
                }
            }
        }

        private void SetAvatar(DepartmentNode node)
        {
            if (node.Type == DepartmentNodeType.Member)
            {
                node.Avatar = new TethysAvatar
                {
                    DisplayName = node.Addition.DisplayName,
                    Background = AvatarHelper.GetColorBrush(node.Addition.DisplayName),
                    Source = AvatarHelper.GetAvatarBitmap(node.Addition.Avatar, AvatarSize.X40, FromType.User)
                };
                _departmentNodeList.Add(node);
            }
            else if (node.Type == DepartmentNodeType.Department)
            {
                foreach (var item in node.Children)
                {
                    SetAvatar(item);
                }
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            OnCloseButtonClick?.Invoke(this);
        }
        private void PrimaryButton_Click(object sender, RoutedEventArgs e)
        {
            OnPrimaryButtonClick?.Invoke(this);
        }


        //private void SelectedAvatars_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        //{
        //    if (_departmentNodeList != null)
        //    {
        //        // 每次更新一个
        //        //void SetIsSelected(bool isSelected)
        //        //{
        //        //    var item = e.NewItems.Cast<TethysAvatar>().First();
        //        //    var node = _departmentNodeList.Single(n => n.Id == item.Id);
        //        //    node.IsSelected = isSelected;
        //        //}

        //        //switch (e.Action)
        //        //{
        //        //    case NotifyCollectionChangedAction.Add:
        //        //        SetIsSelected(true);
        //        //        break;
        //        //    case NotifyCollectionChangedAction.Remove:
        //        //        SetIsSelected(false);
        //        //        break;
        //        //    case NotifyCollectionChangedAction.Reset:
        //        //        {
        //        //            foreach (var item in _departmentNodeList)
        //        //            {
        //        //                item.IsSelected = false;
        //        //            }
        //        //        }
        //        //        break;
        //        //}

        //        // 每次更新所有的IsSelected
        //        //foreach (var item in SelectedAvatars)
        //        //{
        //        //    var node = _departmentNodeList.Single(n => n.Id == item.Id);
        //        //    node.IsSelected = true;
        //        //}
        //        //foreach (var item in UnSelectedAvatars)
        //        //{
        //        //    var node = _departmentNodeList.SingleOrDefault(n => n.Id == item.Id);
        //        //    if (node != null)
        //        //    {
        //        //        node.IsSelected = false;
        //        //    }
        //        //}
        //    }
        //}
    }
}
