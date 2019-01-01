using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Easyfood_Xamarin.Model
{
    public class Restoran
    {
        public int RestoranID { get; set; }
        public System.Guid Sifra { get; set; }
        public string Naziv { get; set; }
        public byte[] Slika { get; set; }
        public string Opis { get; set; }
        public string Email { get; set; }
        public string WebUrl { get; set; }
        public string Telefon { get; set; }
        public Nullable<decimal> MinimalnaCijenaNarudzbe { get; set; }
        public string Adresa { get; set; }
        public int BlokID { get; set; }
        public Nullable<int> GradID { get; set; }
        public Nullable<int> RestoranStatusID { get; set; }
        public string StatusNaziv { get; set; }
        public string AdresaFull { get; set; }
        public string BlokNaziv { get; set; }
        public string GradNaziv { get; set; }
        public Nullable<int> PromStatusKorisnikID { get; set; }
        public string PromStatusUsername { get; set; }
        public string PromStatusImePrezime { get; set; }

        // Dodatno
        public string BlokGrad {
            get
            {
                return this.BlokNaziv + ", " + this.GradNaziv;
            }
        }
    }
}
