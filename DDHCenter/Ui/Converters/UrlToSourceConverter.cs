using System;
using System.Windows.Media.Imaging;

namespace DDHCenter.Ui.Converters
{
    public class UrlToSourceConverter : BaseValueConverter<UrlToSourceConverter>
    {
        public UrlToSourceConverter()
        {

        }

        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return null;
            string imageUrl = (string)value;
            if (imageUrl == "") return null;
            BitmapImage logo = new BitmapImage(new Uri(imageUrl, UriKind.RelativeOrAbsolute));
            return logo;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
