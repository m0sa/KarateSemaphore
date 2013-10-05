using System;
using System.Globalization;
using System.Windows.Data;

namespace KarateSemaphore.Phone
{
    public class LTRConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var str = parameter + "";
            if (str == "rotate")
            {
                return (bool)value ? 180.0 : 0.0;
            }
            if (str == "scale")
            {
                return (bool)value ? -1 : 1;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}