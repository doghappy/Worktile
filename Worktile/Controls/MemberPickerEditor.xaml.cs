using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using Worktile.Enums;
using Worktile.Views.Message;

namespace Worktile.Controls
{
    public sealed partial class MemberPickerEditor : UserControl, INotifyPropertyChanged
    {
        public MemberPickerEditor()
        {
            InitializeComponent();
            Avatars = new ObservableCollection<TethysAvatar>();
            SelectedAvatars = new ObservableCollection<TethysAvatar>();
            SuggestAvatars = new ObservableCollection<TethysAvatar>();
            foreach (var item in DataSource.Team.Members)
            {
                if (item.Role != 5)
                {
                    Avatars.Add(new TethysAvatar
                    {
                        DisplayName = item.DisplayName,
                        Background = AvatarHelper.GetColorBrush(item.DisplayName),
                        Source = AvatarHelper.GetAvatarBitmap(item.Avatar, AvatarSize.X40, FromType.User),
                        DisplayNamePinyin = item.DisplayNamePinyin.Split(',').ToArray()
                    });
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

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
        public string Title { get; set; }

        public ObservableCollection<TethysAvatar> Avatars { get; }
        public ObservableCollection<TethysAvatar> SelectedAvatars { get; }
        public ObservableCollection<TethysAvatar> SuggestAvatars { get; }

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var item = e.ClickedItem as TethysAvatar;
            Avatars.Remove(item);
            SelectedAvatars.Add(item);
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as AppBarButton;
            var item = btn.DataContext as TethysAvatar;
            Avatars.Add(item);
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
                    var items = Avatars.Where(a => a.DisplayName.Contains(text) || a.DisplayNamePinyin.Any(p => p.Contains(text, StringComparison.CurrentCultureIgnoreCase)));
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
                Avatars.Remove(item);
                SuggestAvatars.Remove(item);
            }
        }

        private void AddMembers_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in Avatars)
            {
                SelectedAvatars.Add(item);
            }
            Avatars.Clear();
            SuggestAvatars.Clear();
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in SelectedAvatars)
            {
                Avatars.Add(item);
            }
            SelectedAvatars.Clear();
        }
    }
}
