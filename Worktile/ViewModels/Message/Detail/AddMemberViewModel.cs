using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Worktile.Models.Member;
using System.Linq;
using System;

namespace Worktile.ViewModels.Message.Detail
{
    class AddMemberViewModel : ViewModel, INotifyPropertyChanged
    {
        public AddMemberViewModel(List<Member> members)
        {
            _members = members;
            Members = new ObservableCollection<Member>();
            members.ForEach(i => Members.Add(i));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private readonly List<Member> _members;

        public ObservableCollection<Member> Members { get; }

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

        protected override void OnPropertyChanged([CallerMemberName] string prop = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        private void OnQueryTextChanged()
        {
            Members.Clear();
            if (string.IsNullOrWhiteSpace(QueryText))
            {
                _members.ForEach(i => Members.Add(i));
            }
            else
            {
                string text = QueryText.Trim();
                _members.ForEach(m =>
                {
                    if (m.TethysAvatar.DisplayName.Contains(text, StringComparison.CurrentCultureIgnoreCase) || m.TethysAvatar.DisplayNamePinyin.Any(n => n.Contains(text, StringComparison.CurrentCultureIgnoreCase)))
                    {
                        Members.Add(m);
                    }
                });
            }
        }
    }
}
