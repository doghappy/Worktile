﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Worktile.ApiModels.ApiPinnedMessages;
using Worktile.Common;
using Worktile.Common.WtRequestClient;
using Worktile.Enums;
using Visibility = Windows.UI.Xaml.Visibility;

namespace Worktile.Views.Message
{
    public sealed partial class PinnedPage : Page, INotifyPropertyChanged
    {
        public PinnedPage()
        {
            InitializeComponent();
            Messages = new IncrementalCollection<Message>(LoadMessagesAsync);
            SelectionMode = ListViewSelectionMode.None;
            SingleButtonVisibility = Visibility.Visible;
            MultipleButtonVisibility = Visibility.Visible;
        }

        Session _session;

        public event PropertyChangedEventHandler PropertyChanged;

        private bool _isActive;
        public bool IsActive
        {
            get => _isActive;
            set
            {
                if (_isActive != value)
                {
                    _isActive = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsActive)));
                }
            }
        }

        private Visibility _singleButtonVisibility;
        public Visibility SingleButtonVisibility
        {
            get => _singleButtonVisibility;
            set
            {
                if (_singleButtonVisibility != value)
                {
                    _singleButtonVisibility = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SingleButtonVisibility)));
                }
            }
        }

        private Visibility _multipleButtonVisibility;
        public Visibility MultipleButtonVisibility
        {
            get => _multipleButtonVisibility;
            set
            {
                if (_multipleButtonVisibility != value)
                {
                    _multipleButtonVisibility = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MultipleButtonVisibility)));
                }
            }
        }

        private ListViewSelectionMode _selectionMode;
        public ListViewSelectionMode SelectionMode
        {
            get => _selectionMode;
            set
            {
                if (_selectionMode != value)
                {
                    _selectionMode = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectionMode)));
                }
            }
        }

        public IncrementalCollection<Message> Messages { get; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _session = e.Parameter as Session;
        }

        string _anchor;

        private async Task<IEnumerable<Message>> LoadMessagesAsync()
        {
            IsActive = true;
            const int SIZE = 10;
            var list = new List<Message>();
            string url = $"/api/pinneds?session_id={_session.Id}&anchor={_anchor}&size={SIZE}";
            var client = new WtHttpClient();
            var data = await client.GetAsync<ApiPinnedMessages>(url);
            if (data.Data.Pinneds.Any())
            {
                _anchor = data.Data.Pinneds.Last().Id;
                if (data.Data.Batch < SIZE)
                {
                    Messages.HasMoreItems = false;
                }
                foreach (var item in data.Data.Pinneds)
                {
                    var msg = new Message
                    {
                        Avatar = new TethysAvatar
                        {
                            DisplayName = item.Reference.From.DisplayName,
                            Source = AvatarHelper.GetAvatarBitmap(item.Reference.From.Avatar, AvatarSize.X80, item.Reference.From.Type),
                            Foreground = new SolidColorBrush(Colors.White)
                        },
                        Content = ChatPage.GetContent(item.Reference),
                        Time = item.Reference.CreatedAt,
                        Type = item.Reference.Type
                    };
                    if (Path.GetExtension(item.Reference.From.Avatar).ToLower() == ".png")
                        msg.Avatar.Background = new SolidColorBrush(Colors.White);
                    else
                        msg.Avatar.Background = AvatarHelper.GetColorBrush(item.Reference.From.DisplayName);
                    list.Add(msg);
                }
            }
            else
            {
                Messages.HasMoreItems = false;
            }
            IsActive = false;
            return list;
        }

        private void SingleSelectButton_Click(object sender, RoutedEventArgs e)
        {
            SingleButtonVisibility = Visibility.Collapsed;
            MultipleButtonVisibility = Visibility.Visible;
            SelectionMode = ListViewSelectionMode.Single;
        }

        private void MultipleSelectButton_Click(object sender, RoutedEventArgs e)
        {
            MultipleButtonVisibility = Visibility.Collapsed;
            SingleButtonVisibility = Visibility.Visible;
            SelectionMode = ListViewSelectionMode.Multiple;
        }
    }
}
