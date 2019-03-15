using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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
using Worktile.Models;
using Worktile.Models.Message.Session;
using Worktile.Operators.Message;

namespace Worktile.Views.Message.Dialog
{
    public sealed partial class FileShareDialg : ContentDialog, INotifyPropertyChanged
    {
        public FileShareDialg()
        {
            InitializeComponent();
            Avatars = new ObservableCollection<GroupWrapper>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public Func<string, Task<bool>> SendMessage { get; set; }
        public string FileId { get; set; }

        ObservableCollection<GroupWrapper> Avatars { get; }

        private string _message;
        public string Message
        {
            get => _message;
            set
            {
                if (_message != value)
                {
                    _message = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Message)));
                }
            }
        }

        private TethysAvatar _selectedItem;
        public TethysAvatar SelectedItem
        {
            get => _selectedItem;
            set
            {
                if (_selectedItem != value)
                {
                    _selectedItem = value;
                    IsPrimaryButtonEnabled = value != null;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedItem)));
                }
            }
        }

        private void ContentDialog_Loaded(object sender, RoutedEventArgs e)
        {
            var avatars = new List<TethysAvatar>();
            avatars.AddRange(DataSource.Team.Members.Where(m => m.IsTrueMember()).Select(m => m.TethysAvatar));
            avatars.AddRange(DataSource.JoinedChannels.Select(c => c.TethysAvatar));

            var group = avatars//.Where(a => a.DisplayName == "测试人员")
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
            //MasterOperator.ViewModel.Sessions.fir
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if (SelectedItem != null)
            {
                //string url = $"api/entities/{FileId}/share";
                //var req = new
                //{
                //    ref_id=SelectedItem."5c6625cc5d0bcf5cf13bfb40"
                //};
            }
            /*
             * var data = await WtHttpClient.PostAsync<ApiDataResponse<MemberSession>>("/api/session", new { uid = avatar.Id });
            if (data.Code == 200)
            {
                CreateNewSession(data.Data);
            }
            */
        }
    }
}
