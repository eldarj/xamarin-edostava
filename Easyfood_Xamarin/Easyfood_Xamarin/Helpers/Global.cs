using Easyfood_Xamarin.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Easyfood_Xamarin.Helpers
{
    class Global
    {
        public static Narucilac Narucilac { get; set; }
        public static Narudzba narudzba { get; set; }

        public static void AddHrana(Hrana hrana)
        {
            if (narudzba.HranaStavke.Any(s => s.HranaID == hrana.HranaID))
            {
                ++narudzba.HranaStavke.Where(h => h.HranaID == hrana.HranaID).Single().Kolicina;
            }
            else
            {
                narudzba.HranaStavke.Add(new StavkaNarudzbe
                {
                    NarudzbaID = narudzba.NarudzbaID,
                    HranaID = hrana.HranaID,
                    Kolicina = 1
                });
            }
        }
        public static void RemoveHrana(Hrana hrana)
        {
            if (narudzba.HranaStavke.Any(s => s.HranaID == hrana.HranaID))
            {
                StavkaNarudzbe s = narudzba.HranaStavke.Where(h => h.HranaID == hrana.HranaID).Single();
                if (s.Kolicina > 1)
                {
                    --s.Kolicina;
                }
                else
                {
                    narudzba.HranaStavke.Remove(s);
                }
            }
        }
    }
}
