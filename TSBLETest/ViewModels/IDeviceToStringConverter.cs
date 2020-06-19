using System;
using System.Globalization;
using Plugin.BLE.Abstractions.Contracts;
using Xamarin.Forms;

namespace TSBLETest.ViewModels
{
    public class IDeviceToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            IDevice device = value as IDevice;
            if(device != null)
            { 
                string fooString = device.Id.ToString().Substring(device.Id.ToString().LastIndexOf("-") + 1);
                if (string.IsNullOrEmpty(fooString))
                    return string.Empty;
                else
                    return fooString;
            }
            else
                return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
