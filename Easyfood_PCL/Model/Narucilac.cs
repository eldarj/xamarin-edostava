using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Easyfood_Xamarin.Model
{
    public class Narucilac
    {
        public int KorisnikID { get; set; }
        public string Username { get; set; }
        public string LozinkaHash { get; set; }
        public string LozinkaSalt { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string Email { get; set; }
        public string Telefon { get; set; }
        public System.DateTime DatumRegistracije { get; set; }
        public string Adresa { get; set; }
        public Nullable<int> BlokID { get; set; }
        public int UlogaKorisnikaID { get; set; }
    }
}
