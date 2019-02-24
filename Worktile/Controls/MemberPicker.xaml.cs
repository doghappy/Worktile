using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Worktile.Views.Message;

namespace Worktile.Controls
{
    public sealed partial class MemberPicker : UserControl
    {
        public MemberPicker()
        {
            InitializeComponent();
            SelectedAvatars = new ObservableCollection<TethysAvatar>();
        }

        public ObservableCollection<TethysAvatar> SelectedAvatars { get; }

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
