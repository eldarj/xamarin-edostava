using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Easyfood_Xamarin.Model
{
    public class Hrana
    {
        public int HranaID { get; set; }
        public Nullable<System.Guid> Sifra { get; set; }
        public string Naziv { get; set; }
        public double Cijena { get; set; }
        public string Opis { get; set; }
        public byte[] Slika { get; set; }
        public byte[] SlikaThumb { get; set; }
        public int TipKuhinjeID { get; set; }
        public string TipKuhinjeNaziv { get; set; }
        public Nullable<int> RestoranID { get; set; }
        public string RestoranNaziv { get; set; }
    }
}
