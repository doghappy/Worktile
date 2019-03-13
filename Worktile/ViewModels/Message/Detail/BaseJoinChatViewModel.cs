using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Worktile.Models.Message.Session;

namespace Worktile.ViewModels.Message.Detail
{
    abstract class BaseJoinChatViewModel : ViewModel
    {
        public BaseJoinChatViewModel(MasterViewModel masterViewModel)
        {
            MasterViewModel = masterViewModel;
        }

        protected MasterViewModel MasterViewModel { get; }

        private string _queryText;
        public string QueryText
        {
            get => _queryText;
            set
            {
                if (_queryText != value)
                {
                    _queryText = value;
                    OnQueryTextChanged();
                    OnPropertyChanged();
                }
            }
        }

        protected abstract void OnQueryTextChanged();

        protected void CreateNewSession(ISession session)
        {
            var ss = MasterViewModel.Sessions.SingleOrDefault(s => s.Id == session.Id);
            if (ss == null)
            {
                MasterViewModel.Sessions.Insert(0, session);
                MasterViewModel.SelectedSession = session;
            }
            else
            {
                MasterViewModel.Sessions.Remove(ss);
                MasterViewModel.Sessions.Insert(0, ss);
                MasterViewModel.SelectedSession = ss;
            }
        }
    }
}
