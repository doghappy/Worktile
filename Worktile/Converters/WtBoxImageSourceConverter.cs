﻿using System;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;
using Worktile.Main;

namespace Worktile.Converters
{
    class WtBoxImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value != null)
            {
                string uri = $"{MainViewModel.Box.BaseUrl}/entities/{value.ToString()}/from-s3?team_id={MainViewModel.TeamId}";
                return new BitmapImage
                {
                    UriSource = new Uri(uri)
                };
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
