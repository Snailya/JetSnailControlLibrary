using System;
using System.Globalization;
using System.Windows;

namespace JetSnailControlLibrary.WPF
{
    /// <summary>
    ///     A converter that takes in a number and returns a <see cref="Visibility" />
    /// </summary>
    internal class DivideValueConverter : BaseValueConverter<DivideValueConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            if (parameter != null)
                return (double) value / double.Parse((string) parameter);
            return null;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}