using System;
using System.Globalization;
using Xamarin.Forms;

namespace TSBLETest.ViewModels
{
    public class ImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int result  = (int)value % 10;

            if (result != 0)
                return "img0" + result.ToString()  + ".gif";
            else
                return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
