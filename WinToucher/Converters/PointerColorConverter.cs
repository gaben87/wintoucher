using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace WinToucher.Converters
{
    public class PointerColorConverter : IValueConverter
    {
        private readonly Color[] _colors =
        {
            Color.FromArgb(0x80, 0x00, 0xff, 0x00),
            Color.FromArgb(0x80, 0xff, 0x00, 0xff),
            Color.FromArgb(0x80, 0x00, 0xff, 0xff),
        };

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int index)
            {
                return new SolidColorBrush(_colors[index]);
            }
            return Brushes.Black;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}