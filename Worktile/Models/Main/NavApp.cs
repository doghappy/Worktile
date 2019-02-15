using System.ComponentModel;
using Windows.UI.Xaml;

namespace Worktile.Models.Main
{
    public class NavApp : INotifyPropertyChanged
    {
        public NavApp()
        {
            _state1Visibility = Visibility.Visible;
            _state2Visibility = Visibility.Collapsed;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public string Id { get; set; }
        public string Label { get; set; }
        public string Glyph { get; set; }
        public string SelectedGlyph { get; set; }

        private Visibility _state1Visibility;
        public Visibility State1Visibility
        {
            get => _state1Visibility;
            set
            {
                if (_state1Visibility != value)
                {
                    _state1Visibility = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(State1Visibility)));
                }
            }
        }

        private Visibility _state2Visibility;
        public Visibility State2Visibility
        {
            get => _state2Visibility;
            set
            {
                if (_state2Visibility != value)
                {
                    _state2Visibility = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(State2Visibility)));
                }
            }
        }
    }
}
