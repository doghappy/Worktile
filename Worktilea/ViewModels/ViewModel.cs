using System.Runtime.CompilerServices;

namespace Worktile.ViewModels
{
    public abstract class ViewModel
    {
        private bool _isActive;
        public bool IsActive
        {
            get => _isActive;
            set
            {
                if (_isActive != value)
                {
                    _isActive = value;
                    OnPropertyChanged();
                }
            }
        }

        protected abstract void OnPropertyChanged([CallerMemberName]string prop = null);
    }
}
