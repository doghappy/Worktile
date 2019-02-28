using System.ComponentModel;
using Windows.UI.Xaml.Controls;
using Worktile.Common;
using Worktile.Enums.Message;

namespace Worktile.Controls.Message
{
    public sealed partial class ImageMessage : UserControl, INotifyPropertyChanged
    {
        public ImageMessage()
        {
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private string _source;
        private string Source
        {
            get => _source;
            set
            {
                if (_source != value)
                {
                    _source = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Source)));
                }
            }
        }

        private string _placeholderSource;
        private string PlaceholderSource
        {
            get => _placeholderSource;
            set
            {
                if (_placeholderSource != value)
                {
                    _placeholderSource = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PlaceholderSource)));
                }
            }
        }

        private Models.Message.Message _message;
        public Models.Message.Message Message
        {
            get => _message;
            set
            {
                if (_message != value && value.Type == MessageType.Image)
                {
                    _message = value;
                    PlaceholderSource = WtFileHelper.GetThumbnailUrl(value.Body.Attachment.Addition.Thumbnail);
                    Source = WtFileHelper.GetS3FileUrl(value.Body.Attachment.Id);
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Message)));
                }
            }
        }
    }
}
