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
using Worktile.Common;
using Worktile.Main;
using Worktile.Models;

namespace Worktile
{
    public sealed partial class TestPage : Page
    {
        public TestPage()
        {
            InitializeComponent();
            Members = new ObservableCollection<User>();
            SelectedMembers = new ObservableCollection<User>();
            AutoSuggestBoxItems = new ObservableCollection<User>();
        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadMembers();
        }

        private void LoadMembers()
        {
            foreach (var item in MainViewModel.Members)
            {
                if (item.Role != UserRoleType.Bot)
                {
                    Members.Add(item);
                }
            }
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
    }
}
