using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Worktile.Controls.Emoji;

namespace Worktile.Controls
{
    public sealed partial class EmojiPicker : UserControl, INotifyPropertyChanged
    {
        public EmojiPicker()
        {
            InitializeComponent();
            Pages = new List<EmojiPage>
            {
                new EmojiPage
                {
                    Header = "😀",
                    Page = typeof(EmojiSmileysPage)
                },
                new EmojiPage
                {
                    Header = "👶",
                    Page = typeof(PeopleFantasyPage)
                },
                new EmojiPage
                {
                    Header = "🧥",
                    Page = typeof(ClothingAccessoriesPage)
                },
                new EmojiPage
                {
                    Header = "👶🏻",
                    Page = typeof(PaleEmojisPage)
                },
                new EmojiPage
                {
                    Header = "👶🏼",
                    Page = typeof(CreamWhiteEmojisPage)
                },
                new EmojiPage
                {
                    Header = "👶🏽",
                    Page = typeof(ModerateBrownEmojisPage)
                },
                new EmojiPage
                {
                    Header = "👶🏾",
                    Page = typeof(DarkBrownEmojisPage)
                },
                new EmojiPage
                {
                    Header = "👶🏿",
                    Page = typeof(BlackEmojisPage)
                },
                new EmojiPage
                {
                    Header = "🐶",
                    Page = typeof(AnimalsNaturePage)
                },
                new EmojiPage
                {
                    Header = "🍏",
                    Page = typeof(FoodDrinkPage)
                },
                new EmojiPage
                {
                    Header = "⚽️",
                    Page = typeof(ActivitySportsPage)
                },
                new EmojiPage
                {
                    Header = "🚗",
                    Page = typeof(TravelPlacesPage)
                },
                new EmojiPage
                {
                    Header = "⌚️",
                    Page = typeof(ObjectsPage)
                },
                new EmojiPage
                {
                    Header = "❤️",
                    Page = typeof(SymbolsPage)
                },
                new EmojiPage
                {
                    Header = "🥰",
                    Page = typeof(NewEmojisPage)
                },
            };
            _selectEmoji = emoji => OnEmojiSelected?.Invoke(emoji);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public event Action<string> OnEmojiSelected;

        private Action<string> _selectEmoji;

        List<EmojiPage> Pages { get; }

        EmojiPage _selectedPage;
        EmojiPage SelectedPage
        {
            get => _selectedPage;
            set
            {
                if (_selectedPage != value)
                {
                    _selectedPage = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedPage)));
                    ContentFrame.Navigate(value.Page, _selectEmoji);
                }
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            SelectedPage = Pages.First();
        }
    }

    class EmojiPage
    {
        public string Header { get; set; }
        public Type Page { get; set; }
    }
}
