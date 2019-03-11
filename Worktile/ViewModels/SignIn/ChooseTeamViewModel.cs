using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml.Controls;

namespace Worktile.ViewModels.SignIn
{
    class ChooseTeamViewModel : SignInViewModel, INotifyPropertyChanged
    {
        public ChooseTeamViewModel(Frame frame)
        {
            Frame = frame;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected override void OnPropertyChanged([CallerMemberName] string prop = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
