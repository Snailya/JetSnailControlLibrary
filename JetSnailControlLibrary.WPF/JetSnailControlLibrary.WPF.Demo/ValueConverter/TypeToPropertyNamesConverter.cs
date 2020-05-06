using System;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace JetSnailControlLibrary.WPF.Demo
{
    public class TypeToPropertyNamesConverter : BaseValueConverter<TypeToPropertyNamesConverter>
    {
        public override object Convert(dynamic value, Type targetType, object parameter, CultureInfo culture)
        {
            Type[] genericArguments = value.GetType().GetGenericArguments();

            if (genericArguments.Length != 1)
                throw new ArgumentException(
                    "Invalid property type, the return type has multiple generic arguments");

            // get the public properties
            PropertyInfo[] propInfos = genericArguments.First().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            // return the public properties

            return propInfos.Select(propInfo => propInfo.Name).ToList();
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}