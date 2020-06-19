using System;
using System.Globalization;
using Xamarin.Forms;

namespace TSBLETest.ViewModels
{
    public class ImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool result = (bool)value;
            if (result)
                return "animal.gif";
            else
                return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
