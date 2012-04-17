using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace KarateSemaphore
{
    [ValueConversion(typeof (TimeSpan), typeof (String))]
    public class MinutesSecondsConverter : IValueConverter
    {
        private static readonly string[] Format = new[]
                                                      {
                                                          "m\\:ss",
                                                          "mm\\:ss",
                                                          "mm\\:s",
                                                          "m\\:s"
                                                      };

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter,
                              CultureInfo culture)
        {
            return ((TimeSpan) value).ToString(Format[0]);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {

            TimeSpan result;
            var stringValue = value + string.Empty;
            stringValue = stringValue.Trim();
            if(!TimeSpan.TryParseExact(stringValue, Format, CultureInfo.CurrentUICulture, out result))
            {
                return DependencyProperty.UnsetValue;
            }
            return result;
        }

        #endregion
    }
}