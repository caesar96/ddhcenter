using System;
using System.Windows;

namespace DDHCenter.Ui.Converters
{
    public class BooleanToVisibilityConverterInverse : BaseValueConverter<BooleanToVisibilityConverterInverse>
    {
        public BooleanToVisibilityConverterInverse()
        {

        }

        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (bool)value ? Visibility.Collapsed : Visibility.Visible;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
