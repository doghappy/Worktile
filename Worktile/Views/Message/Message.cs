using System;
using System.ComponentModel;
using Windows.UI.Xaml;

namespace Worktile.Views.Message
{
    public class Message : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string Id { get; set; }
        public TethysAvatar Avatar { get; set; }
        public MessageType Type { get; set; }
        public DateTime Time { get; set; }
        public string Content { get; set; }
        public bool IsShowPin => Type != MessageType.Activity;
        //public bool IsShowPin { get; set; }
        //private bool _isShowPin;
        //public bool IsShowPin
        //{
        //    get => _isShowPin;
        //    set
        //    {
        //        if (_isShowPin != value)
        //        {
        //            _isShowPin = value;
        //            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsShowPin)));
        //        }
        //    }
        //}

        private bool _isPinned;
        public bool IsPinned
        {
            get => _isPinned;
            set
            {
                if (value)
                {
                    PinVisibility = Visibility.Collapsed;
                    UnPinVisibility = Visibility.Visible;
                }
                else
                {
                    PinVisibility = Visibility.Visible;
                    UnPinVisibility = Visibility.Collapsed;
                }
                if (_isPinned != value)
                {
                    _isPinned = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsPinned)));
                }
            }
        }

        Visibility _pinVisibility;
        public Visibility PinVisibility
        {
            get => _pinVisibility;
            set
            {
                if (_pinVisibility != value)
                {
                    _pinVisibility = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PinVisibility)));
                }
            }
        }

        Visibility _unPinVisibility;
        public Visibility UnPinVisibility
        {
            get => _unPinVisibility;
            set
            {
                if (_unPinVisibility != value)
                {
                    _unPinVisibility = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UnPinVisibility)));
                }
            }
        }
    }
}
