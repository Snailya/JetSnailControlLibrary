using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media;

namespace JetSnailControlLibrary.WPF
{
    internal class ZeroToEmptyStringConverter : BaseValueConverter<ZeroToEmptyStringConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (int) value == 0 ? string.Empty : value.ToString();
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}