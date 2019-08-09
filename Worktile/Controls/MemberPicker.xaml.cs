using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Worktile.Common;
using Worktile.Main;
using Worktile.Models;

namespace Worktile.Controls
{
    public sealed partial class MemberPicker : UserControl
    {
        public MemberPicker()
        {
            InitializeComponent();
            Members = new ObservableCollection<User>();
            SelectedMembers = new ObservableCollection<User>();
            AutoSuggestBoxItems = new ObservableCollection<User>();
        }

        public ObservableCollection<User> Members { get; }
        public ObservableCollection<User> SelectedMembers { get; }
        public ObservableCollection<User> AutoSuggestBoxItems { get; }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (var item in e.AddedItems)
            {
                SelectedMembers.Add(item as User);
            }
            foreach (var item in e.RemovedItems)
            {
                SelectedMembers.Remove(item as User);
            }
        }

        private void SelectAll_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in Members)
            {
                if (!MemberListView.SelectedItems.Contains(item))
                {
                    MemberListView.SelectedItems.Add(item);
                }
            }
        }

        private void ClearSelection_Click(object sender, RoutedEventArgs e)
        {
            MemberListView.SelectedItems.Clear();
        }

        private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                AutoSuggestBoxItems.Clear();
                if (sender.Text.Trim() != string.Empty)
                {
                    foreach (var item in MainViewModel.Members)
                    {
                        if (item.Role != UserRoleType.Bot)
                        {
                            if (item.DisplayName.Contains(sender.Text)
                                || item.DisplayNamePinYin.Contains(sender.Text)
                                || item.Name.Contains(sender.Text))
                            {
                                AutoSuggestBoxItems.Add(item);
                            }
                        }
                    }
                }
            }
        }

        private void AutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            MemberListView.SelectedItems.Add(args.ChosenSuggestion);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var mainPage = SharedData.GetMainPage();
            foreach (var item in mainPage.Members)
            {
                if (item.Role != UserRoleType.Bot)
                {
                    Members.Add(item);
                }
            }
        }
    }
}
