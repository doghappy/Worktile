using System.ComponentModel;
using Windows.UI.Xaml.Controls;
using Worktile.Models.Member;

namespace Worktile.Controls
{
    public sealed partial class MemberInfoPane : UserControl, INotifyPropertyChanged
    {
        public MemberInfoPane()
        {
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private Member _member;
        public Member Member
        {
            get => _member;
            set
            {
                if (_member != value)
                {
                    _member = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Member)));
                }
            }
        }
    }
}
