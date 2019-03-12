using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Worktile.ApiModels;
using Worktile.Common;
using Worktile.Common.WtRequestClient;
using Worktile.Enums;
using Worktile.Models;
using Worktile.Models.Message.Session;

namespace Worktile.ViewModels.Message.Detail
{
    public class CreateChannelViewModel : ViewModel, INotifyPropertyChanged
    {
        public CreateChannelViewModel()
        {
            SelectedAvatars = new ObservableCollection<TethysAvatar>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<TethysAvatar> SelectedAvatars { get; }

        private string _color;
        public string Color
        {
            get => _color;
            set
            {
                if (_color != value)
                {
                    _color = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _description;
        public string Description
        {
            get => _description;
            set
            {
                if (_description != value)
                {
                    _description = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _channelName;
        public string ChannelName
        {
            get => _channelName;
            set
            {
                if (_channelName != value)
                {
                    _channelName = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _isPrivate;
        public bool IsPrivate
        {
            get => _isPrivate;
            set
            {
                if (_isPrivate != value)
                {
                    _isPrivate = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _showError;
        public bool ShowError
        {
            get => _showError;
            set
            {
                if (_showError != value)
                {
                    _showError = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _errorText;
        public string ErrorText
        {
            get => _errorText;
            set
            {
                if (_errorText != value)
                {
                    _errorText = value;
                    OnPropertyChanged();
                }
            }
        }

        protected override void OnPropertyChanged([CallerMemberName] string prop = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public async Task<bool> CreateAsync()
        {
            var uids = SelectedAvatars.Select(a => a.Id);
            var req = new
            {
                name = ChannelName.Trim(),
                color = Color,
                desc = Description,
                default_uids = string.Join(',', uids),
                visibility = IsPrivate ? WtVisibility.Private : WtVisibility.Public
            };
            var res = await WtHttpClient.PostAsync<ApiDataResponse<ChannelSession>>("api/channel", req);
            return res.Code == 200;
        }
    }
}
