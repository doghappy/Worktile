using System;
using System.ComponentModel;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using WtMessage = Worktile.Message.Models;

namespace Worktile.Controls.Message
{
    public sealed partial class CalendarMessage : UserControl, INotifyPropertyChanged
    {
        public CalendarMessage()
        {
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private WtMessage.Message _message;
        public WtMessage.Message Message
        {
            get => _message;
            set
            {
                if (_message != value && value.Type == WtMessage.MessageType.Calendar)
                {
                    _message = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Message)));
                }
            }
        }
    }

    public class CalendarBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string val = value.ToString();
            if (val == "1")
            {
                //return WtColorHelper.WarningColor1A;
            }
            return new SolidColorBrush(Colors.Transparent);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
