﻿using System.ComponentModel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Worktile.WindowsUI.ViewModels.Project;

namespace Worktile.WindowsUI.Views.Mission
{
    public sealed partial class HomePage : Page, INotifyPropertyChanged
    {
        public HomePage()
        {
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private HomeViewModel viewModel;
        public HomeViewModel ViewModel
        {
            get => viewModel;
            set
            {
                viewModel = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ViewModel)));
            }
        }


        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ViewModel = new HomeViewModel();
            await ViewModel.InitializeAsync();
        }
    }
}