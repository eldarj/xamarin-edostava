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
        public Nullable<int> KorisnikID { get; set; }
        public string Adresa { get; set; }
        public Nullable<int> BlokID { get; set; }
        public string StatusNarudzbe { get; set; }
        public string AdresaFull { get; set; }
        public string BlokNaziv { get; set; }
        public Nullable<int> GradID { get; set; }
        public string GradNaziv { get; set; }

        public List<StavkaNarudzbe> HranaStavke { get; set; }

        public Narudzba()
        {
            Sifra = Guid.NewGuid();
            HranaStavke = new List<StavkaNarudzbe>();
        }
    }
}
