using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Easyfood_PCL.Model
{
    public class Recenzija
    {
        public int RestoranRecenzijaID { get; set; }
        public DateTime Datum { get; set; }
        public string Komentar { get; set; }
        public int Ocjena { get; set; }
        public Nullable<int> KorisnikID { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public int RestoranID { get; set; }
        public string Naziv { get; set; } //RestoranNaziv

        #region FormattedProps
        public string ImePrezime { get { return Ime + " " + Prezime;  } }
        public string OcjenaFormatted { get { return Ocjena != 0 ? "Ocjena: " + Ocjena.ToString() : ""; } }
        public string DatumFormatted { get { return Datum != DateTime.MinValue ? Datum.ToString("dd.MM.yyyy") : ""; } }
        #endregion
    }
}
