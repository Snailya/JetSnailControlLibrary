using System;
using System.Globalization;

namespace JetSnailControlLibrary.WPF
{
    internal class ReserveBooleanConverter : BaseValueConverter<ReserveBooleanConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(bool) value;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}