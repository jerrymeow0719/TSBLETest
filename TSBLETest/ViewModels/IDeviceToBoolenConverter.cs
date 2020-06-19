using System;
using System.Globalization;
using Plugin.BLE.Abstractions.Contracts;
using Xamarin.Forms;

namespace TSBLETest.ViewModels
{
    public class IDeviceToBoolenConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            IDevice device = value as IDevice;
            if (device != null)
            {
                string fooString = device.Id.ToString().Substring(device.Id.ToString().LastIndexOf("-") + 1);
                if (fooString.Length > 0)
                    return true;
                else
                    return false;
            }
            else
                return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
