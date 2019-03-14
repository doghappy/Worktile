using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Worktile.ApiModels;
using Worktile.Common;
using Worktile.Common.Communication;
using Worktile.Enums;
using Worktile.Enums.Message;
using Worktile.Models;
using Worktile.Models.Department;
using Worktile.Views.Message;
using Worktile.Models.Member;
using Worktile.Common.Extensions;

namespace Worktile.Controls
{
    public sealed partial class MemberPickerEditor : UserControl, INotifyPropertyChanged
    {
        public MemberPickerEditor()
        {
            InitializeComponent();
            UnSelectedAvatars = new ObservableCollection<TethysAvatar>();
            SelectedAvatars = new ObservableCollection<TethysAvatar>();
            DepartmentNodes = new ObservableCollection<DepartmentNode>();
        }

        public event Action<MemberPickerEditor> OnPrimaryButtonClick;
        public event Action<MemberPickerEditor> OnCloseButtonClick;

        public event PropertyChangedEventHandler PropertyChanged;

        private string _queryText;
        public string QueryText
        {
            get => _queryText;
            set
            {
                if (_queryText != value)
                {
                    _queryText = value;
                    OnQueryTextChanged();
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Title)));
                }
            }
        }

        private List<DepartmentNode> _departmentNodeList { get; set; }

        public string Title { get; set; }

        public ObservableCollection<TethysAvatar> UnSelectedAvatars { get; }
        public ObservableCollection<TethysAvatar> SelectedAvatars { get; }
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

        private void OnQueryTextChanged()
        {
            string text = QueryText.Trim();
            UnSelectedAvatars.Clear();
            if (text == string.Empty)
            {
                foreach (var item in DataSource.Team.Members)
                {
                    if (item.IsTrueMember() && !SelectedAvatars.Any(a => a.Name == item.Name))
                    {
                        UnSelectedAvatars.Add(item.TethysAvatar);
                    }
                }
            }
            else
            {
                foreach (var item in DataSource.Team.Members)
                {
                    if (item.IsTrueMember() && !SelectedAvatars.Any(a => a.Name == item.Name)
                        && (item.TethysAvatar.DisplayName.Contains(text, StringComparison.CurrentCultureIgnoreCase) || item.TethysAvatar.DisplayNamePinyin.Any(p => p.Contains(text, StringComparison.CurrentCultureIgnoreCase))))
                    {
                        UnSelectedAvatars.Add(item.TethysAvatar);
                    }
                }
            }
        }

        private void AddMembers_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in UnSelectedAvatars)
            {
                SelectedAvatars.Add(item);
            }
            UnSelectedAvatars.Clear();
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
            // 此控件外部是Flyout，Flyout会缓存内容，此处判断是为了性能。
            if (!UnSelectedAvatars.Any())
            {
                foreach (var item in DataSource.Team.Members)
                {
                    if (item.IsTrueMember())
                    {
                        UnSelectedAvatars.Add(item.TethysAvatar);
                    }
                }
            }

            // 此控件外部是Flyout，Flyout会缓存内容，此处判断是为了性能。
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
