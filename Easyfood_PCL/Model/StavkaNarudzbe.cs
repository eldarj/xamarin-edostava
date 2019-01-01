using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Easyfood_Xamarin.Model
{
    public class StavkaNarudzbe
    {
        public int StavkaNarudzbeID { get; set; }
        public int NarudzbaID { get; set; }
        public int HranaID { get; set; }
        public int Kolicina { get; set; }

        public Hrana Hrana { get; set; }


    }
}
