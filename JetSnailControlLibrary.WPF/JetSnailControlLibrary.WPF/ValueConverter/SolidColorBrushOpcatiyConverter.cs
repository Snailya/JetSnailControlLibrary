using System;
using System.Globalization;
using System.Windows.Media;

namespace JetSnailControlLibrary.WPF
{
    internal class SolidColorBrushOpcatiyConverter : BaseValueConverter<SolidColorBrushOpcatiyConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is SolidColorBrush brush)) return value;

            return double.TryParse(parameter.ToString(), out var opacity)
                ? new SolidColorBrush(brush.Color) {Opacity = opacity}
                : value;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}