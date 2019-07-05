using System;
using System.ComponentModel;
using Windows.Storage;
using Windows.System;
using Windows.UI.Xaml.Controls;
using Worktile.Common;
using WtMessage = Worktile.Message.Models;

namespace Worktile.Controls.Message
{
    public sealed partial class ImageMessage : UserControl, INotifyPropertyChanged
    {
        public ImageMessage()
        {
            InitializeComponent();
            IsButtonEnabled = true;
        }

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

        private bool _isButtonEnabled;
        public bool IsButtonEnabled
        {
            get => _isButtonEnabled;
            set
            {
                if (_isButtonEnabled != value)
                {
                    _isButtonEnabled = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsButtonEnabled)));
                }
            }
        }


        private WtMessage.Message _message;
        public WtMessage.Message Message
        {
            get => _message;
            set
            {
                if (_message != value && value.Type == WtMessage.MessageType.Image)
                {
                    _message = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Message)));
                }
            }
        }

        private async void Button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            //IsActive = true;
            //IsButtonEnabled = false;
            //var folderItem = await ApplicationData.Current.LocalFolder.TryGetItemAsync("MessageImages");
            //if (folderItem == null)
            //{
            //    folderItem = await ApplicationData.Current.LocalFolder.CreateFolderAsync("MessageImages");
            //}
            //var folder = folderItem as StorageFolder;
            //string fileName = Message.Body.Attachment.Id + "." + Message.Body.Attachment.Addition.Ext;
            //var fileItem = await folder.TryGetItemAsync(fileName);
            //StorageFile file;
            //if (fileItem == null)
            //{
            //    string url = UtilityTool.GetS3FileUrl(Message.Body.Attachment.Id);
            //    var buffer = await WtHttpClient.GetByteBufferAsync(url);
            //    file = await folder.CreateFileAsync(fileName);
            //    await FileIO.WriteBufferAsync(file, buffer);
            //}
            //else
            //{
            //    file = fileItem as StorageFile;
            //}
            //await Launcher.LaunchFileAsync(file);
            //IsButtonEnabled = true;
            //IsActive = false;
        }
    }
}
