using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windows.UI.Xaml.Data;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace App1.Converters
{
    public class MyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool val = (bool)value;
            return val? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            Visibility val = (Visibility)value;
            return val == Visibility.Visible ? true : false;
        }
    };

    public class MyConverter2 : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool val = (bool)value;
            return !val ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            Visibility val = (Visibility)value;
            return val == Visibility.Visible ? false : true;
        }
    }
}

