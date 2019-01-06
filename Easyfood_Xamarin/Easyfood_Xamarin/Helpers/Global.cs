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
        public static Narudzba Narudzba { get; set; }
        public static List<Restoran> CurrentOmiljeni { get; set; } = new List<Restoran>();
        public static bool OnUserAuthGoToKorpaFlag { get; set; }

        public static string GetNarucilacAdresa { get { return Narucilac != null ? Narucilac.Adresa : "-";  } }
        public static int? GetNarucilacBlokID { get { return Narucilac?.BlokID; } }

        public static void AddHrana(Hrana hrana)
        {
            if (Narudzba.StavkeNarudzbe.Any(s => s.HranaID == hrana.HranaID))
            {
                ++Narudzba.StavkeNarudzbe.Where(h => h.HranaID == hrana.HranaID).Single().Kolicina;
            }
            else
            {
                Narudzba.StavkeNarudzbe.Add(new StavkaNarudzbe
                {
                    Hrana = hrana,
                    NarudzbaID = Narudzba.NarudzbaID,
                    HranaID = hrana.HranaID,
                    Kolicina = 1
                });
            }
        }
        public static void RemoveHrana(Hrana hrana)
        {
            if (Narudzba.StavkeNarudzbe.Any(s => s.HranaID == hrana.HranaID))
            {
                StavkaNarudzbe s = Narudzba.StavkeNarudzbe.Where(h => h.HranaID == hrana.HranaID).Single();
                if (s.Kolicina > 1)
                {
                    --s.Kolicina;
                }
                else
                {
                    Narudzba.StavkeNarudzbe.Remove(s);
                }
            }
        }
    }
}
