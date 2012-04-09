using System;
using System.Linq;
using System.Globalization;
using System.Windows.Data;

namespace KarateSemaphore
{
    [ValueConversion(typeof(bool), typeof(object), ParameterType = typeof(String))]
    public class TernaryConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var @default = Activator.CreateInstance(targetType);
            var parameters = parameter.ToString().Split(new[] { "||" }, StringSplitOptions.RemoveEmptyEntries).Select(x => System.Convert.ChangeType(x, targetType, CultureInfo.CurrentCulture)).ToArray();
            var boolValue = (bool)value;
            if (boolValue)
            {
                return parameters.ElementAtOrDefault(0) ?? @default;
            }
            else
            {
                return parameters.ElementAtOrDefault(1) ?? @default;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
