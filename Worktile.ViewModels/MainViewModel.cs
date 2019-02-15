//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.ComponentModel;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Windows.UI;
//using Windows.UI.Xaml;
//using Windows.UI.Xaml.Media;
//using Worktile.Models.Main;

//namespace Worktile.ViewModels
//{
//    public class MainViewModel : INotifyPropertyChanged
//    {
//        public MainViewModel()
//        {
//            NavBackground = "#6f76fa";
//            //NavSelectedBackground = WtColorHelper.GetColor("#575df9");
//            NavSelectedBackground = Colors.Red;
//            LogoUrl = "https://s3.cn-north-1.amazonaws.com.cn/lclogo/8dbc28b9-ec0e-4d2e-b66d-ea235525942d_180x180.png";
//            AvatarUrl = "https://s3.cn-north-1.amazonaws.com.cn/lcavatar/7f79f351-408d-459f-b880-620eef734b65_80x80.png";
//            NavApps = new ObservableCollection<NavApp>
//            {
//                new NavApp
//                {
//                    Label = "消息",
//                    Glyph = "\ue618",
//                    SelectedGlyph = "\ue61e"
//                },
//                new NavApp
//                {
//                    Label = "项目",
//                    Glyph = "\ue70d",
//                    SelectedGlyph = "\ue70c"
//                }
//            };
//            NavApp = NavApps.First();
//        }

//        public event PropertyChangedEventHandler PropertyChanged;

//        public string NavBackground { get; }
//        public Color NavSelectedBackground { get; }
//        public string LogoUrl { get; }
//        public string AvatarUrl { get; }

//        public ObservableCollection<NavApp> NavApps { get; }

//        private NavApp _navApp;
//        public NavApp NavApp
//        {
//            get => _navApp;
//            set
//            {
//                if (_navApp != value)
//                {
//                    if (_navApp != null)
//                    {
//                        _navApp.State1Visibility = Visibility.Visible;
//                        _navApp.State2Visibility = Visibility.Collapsed;
//                    }
//                    value.State1Visibility = Visibility.Collapsed;
//                    value.State2Visibility = Visibility.Visible;
//                    _navApp = value;
//                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NavApp)));
//                }
//            }
//        }


//    }
//}
