using System;
using System.Globalization;
using System.Reflection;
using System.Windows.Data;

namespace KarateSemaphore.Phone
{
    public partial class TimeControl 
    {
        public TimeControl()
        {
            InitializeComponent();
        }
    }

    public class TimeDisplayConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var timeSpan = value as TimeSpan?;
            if (targetType == typeof (string) && timeSpan != null)
            {
                var ts = timeSpan.Value;
                var result = string.Format(CultureInfo.InvariantCulture, "{0}:{1}:{2}",
                    ts.Minutes.ToString("00").Substring(0, 2),
                    ts.Seconds.ToString("00").Substring(0, 2),
                    ts.Milliseconds.ToString("00").Substring(0, 2));
                return result;
            }
            if (value is string)
            {
                return value;
            }
            throw new NotSupportedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
