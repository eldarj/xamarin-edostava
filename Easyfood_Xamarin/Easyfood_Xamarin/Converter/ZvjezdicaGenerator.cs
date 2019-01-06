using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace Easyfood_Xamarin.Converter
{
    // Konvertuje odn. generiše ImageButton zvjezdice (u zavisnosti od ocjene) prikazane na komentarima/ocjenama restorana
    class ZvjezdicaGenerator : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Imamo 5 zvjezdica koje hoćemo prikazati kao 'uključene' ili samo tamne...
            // IF: trenutni broj zvjezdice manji od ocjene pokaži kao star.png
            // ELSE: prikaži kao tamnu verziju star_dark.png
            if (System.Convert.ToInt32(parameter) <= System.Convert.ToInt32(value))
            {
                return "star.png";
            } else
            {
                return "star_dark.png";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
