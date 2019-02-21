using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Windows.UI.Xaml.Controls;
using Worktile.Common;
using Worktile.Enums;

namespace Worktile.Views.Message.Dialog
{
    public sealed partial class CreateGroupDialog : ContentDialog, INotifyPropertyChanged
    {
        public CreateGroupDialog()
        {
            InitializeComponent();
            WtVisibilities = new List<WtVisibility>
            {
                new WtVisibility
                {
                    Visibility = Enums.Visibility.Private,
                    Text = "私有：只有加入的成员才能看见此群组",
                },
                new WtVisibility
                {
                    Visibility = Enums.Visibility.Public,
                    Text = "公开：企业所有成员都可以看见此群组",
                }
            };
            SelectedVisibility = WtVisibilities.First();
            Color = WtColorHelper.Map.First().NewColor;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private string _color;
        public string Color
        {
            get => _color;
            set
            {
                if (_color != value)
                {
                    _color = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Color)));
                }
            }
        }

        List<WtVisibility> WtVisibilities { get; }

        WtVisibility _selectedVisibility;
        WtVisibility SelectedVisibility
        {
            get => _selectedVisibility;
            set
            {
                if (_selectedVisibility != value)
                {
                    _selectedVisibility = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedVisibility)));
                }
            }
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }

    class WtVisibility
    {
        public Visibility Visibility { get; set; }
        public string Text { get; set; }
    }
}
