using System.ComponentModel;
using Windows.UI.Xaml.Controls;
using Worktile.Models.Member;
using Worktile.Common.Extensions;
using Worktile.Enums;

namespace Worktile.Controls
{
    public sealed partial class MemberInfoCard : UserControl, INotifyPropertyChanged
    {
        public MemberInfoCard()
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
                    if (value.TethysAvatar == null)
                    {
                        value.ForShowAvatar(AvatarSize.X80);
                    }
                    _member = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Member)));
                }
            }
        }
    }
}
