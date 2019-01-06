using System;
using System.Globalization;
using Xamarin.Forms;

namespace Easyfood_Xamarin.Converter
{
    // Konvertera string.toupper()....
    class LabelCapitalizer : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            string s = value as string;
            return s.ToUpper();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
