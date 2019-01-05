using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Easyfood_Xamarin.Model
{
    public class Narudzba
    {
        public int NarudzbaID { get; set; }
        public System.Guid Sifra { get; set; }
        public System.DateTime DatumVrijeme { get; set; }
        public double UkupnaCijena { get; set; }
        public Nullable<int> NarucilacID { get; set; }
        public Nullable<int> NarudzbaStatusID { get; set; }
        public string StatusNarudzbe { get; set; }
        public string Adresa { get; set; }
        public Nullable<int> BlokID { get; set; }
        public string BlokNaziv { get; set; }
        public Nullable<int> GradID { get; set; }
        public string GradNaziv { get; set; }

        public List<StavkaNarudzbe> StavkeNarudzbe { get; set; }

        #region FormattedProps
        public string SifraString { get { return Sifra.ToString(); } }
        public string DatumVrijemeFormatted { get { return DatumVrijeme.ToString("dd.MM.yyyy"); } }
        public string VrijemeFormatted { get { return DatumVrijeme.ToString("hh:mm"); } }
        public string AdresaBlokGrad { get { return Adresa + " " + BlokNaziv + ", " + GradNaziv; } }
        public string UkupnaCijenaFormatted { get { return String.Format("{0} KM", UkupnaCijena.ToString("0.00")); } }
        #endregion

        public Narudzba()
        {
            StavkeNarudzbe = new List<StavkaNarudzbe>();
        }
    }
}
