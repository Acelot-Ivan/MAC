using System;
using System.Globalization;
using System.Windows.Data;

namespace MAC.Style.Converter
{
    /// <summary>
    /// Преобразование для интерфейса мили секунд в секунды
    /// </summary>
    public class MilliSecToSec : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // ReSharper disable once PossibleNullReferenceException
            var voltValue = (int)value / 1000;
            return voltValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // ReSharper disable once PossibleNullReferenceException
            if (value is string stringValue)
            {
                var milliVoltValue = int.Parse(stringValue) * 1000;
                return milliVoltValue;
            }

            //10000 мСек
            const int minValue = 10000;

            return minValue;

        }
    }
}