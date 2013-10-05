using System;
using System.Globalization;
using System.Windows.Data;
using KarateSemaphore.Core;

namespace KarateSemaphore.Phone
{
    public class PenaltyToIntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return PenaltyToInt((Penalty)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return IntToPenalty((int)(double)value);
        }

        private static int PenaltyToInt(Penalty value)
        {
            return (int)value;
        }

        private static Penalty IntToPenalty(int value)
        {
            if (Enum.IsDefined(typeof (Penalty), value))
            {
                return (Penalty)value;
            }

            throw new ArgumentOutOfRangeException("value");
        }

    }
}