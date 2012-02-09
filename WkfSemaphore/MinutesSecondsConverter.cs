using System;
using System.Globalization;
using System.Windows.Data;

namespace WkfSemaphore
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
            return
                TimeSpan.TryParseExact(value + string.Empty, Format, CultureInfo.CurrentUICulture, out result)
                    ? result
                    : TimeSpan.Zero;
        }

        #endregion
    }
}