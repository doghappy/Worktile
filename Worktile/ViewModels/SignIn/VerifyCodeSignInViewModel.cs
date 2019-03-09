using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Worktile.PageModels;

namespace Worktile.ViewModels.SignIn
{
    class VerifyCodeSignInViewModel : ViewModel, INotifyPropertyChanged
    {
        public VerifyCodeSignInViewModel()
        {
            Areas = new List<WtKeyValue>
            {
                new WtKeyValue("中国 +86", "0086"),
                new WtKeyValue("台湾 +886", "00886"),
                new WtKeyValue("United States +1", "001"),
                new WtKeyValue("日本 +81", "0081")
            };
            SelectedArea = Areas.First();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public List<WtKeyValue> Areas { get; }

        private WtKeyValue _selectedArea;
        public WtKeyValue SelectedArea
        {
            get => _selectedArea;
            set
            {
                if (_selectedArea != value)
                {
                    _selectedArea = value;
                    OnPropertyChanged();
                }
            }
        }

        protected override void OnPropertyChanged([CallerMemberName] string prop = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
