using System.Collections.ObjectModel;
using Windows.UI.Xaml.Controls;
using Worktile.Models;

namespace Worktile.Controls
{
    public sealed partial class MemberPicker : UserControl
    {
        public MemberPicker()
        {
            InitializeComponent();
        }

        public ObservableCollection<TethysAvatar> SelectedAvatars { get; set; }

        private void MemberPickerEditor_OnPrimaryButtonClick(MemberPickerEditor editor)
        {
            SelectedAvatars.Clear();
            foreach (var item in editor.SelectedAvatars)
            {
                SelectedAvatars.Add(item);
            }
            EditorFlyout.Hide();
        }

        private void MemberPickerEditor_OnCloseButtonClick(MemberPickerEditor editor)
        {
            EditorFlyout.Hide();
        }
    }
}
