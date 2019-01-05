using Easyfood_Xamarin.Helpers;
using Easyfood_Xamarin.Model;
using Easyfood_Xamarin.Util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Input;
using Xamarin.Forms;

namespace Easyfood_Xamarin.ViewModels
{
    class KorpaVM : INotifyPropertyChanged
    {
        public class KorpaListItem
        {
            public byte[] Slika { get; set; }
            public string Naziv { get; set; }
            public string NarucenoIzRestorana { get; set; }
            public double Cijena { get; set; }
            public int Kolicina { get; set; }

            // formatted props
            public string CijenaFormatted { get { return String.Format("Cijena {0} KM", Cijena);  } }
            public string KolicinaNazivFormatted { get { return String.Format("x{0} {1}", Kolicina, Naziv); } }
            public string CijenaUkupnoFormatted { get { return String.Format("{0} KM", Cijena * Kolicina); } }
        }
        public INavigation Navigation { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<KorpaListItem> _korpaList { get; set; }
        public ObservableCollection<KorpaListItem> KorpaList {
            get { return _korpaList; }
            set
            {
                _korpaList = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("KorpaList"));
            }
        }

        private string _adresa { get; set; }
        public int? AdresaBlokID { get; set; }
        public bool HasValidAdres { get; set; }
        public bool DisplayInvalidAdresInfo { get { return !HasValidAdres; } }
        public string Adresa
        {
            get { return _adresa; }
            set
            {
                /* Adresu setujemo na text-onchange listener (Pogledaj: KorpaPage.xaml.cs)
                 * pošto taj listener konstanto radi tokom tipkanja, 
                 * HasValidAdres vrijednost ćemo mijenjat samo ukoliko se ona zaista promijeni (u odnosu na staru) */
                _adresa = value;
                bool isAdresValid = Regex.IsMatch(_adresa, @"^[\p{L}\d\ \.\-]{4,}$");
                if (isAdresValid != HasValidAdres)
                {
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("KorpaValid"));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DisplayInvalidAdresInfo"));
                    HasValidAdres = isAdresValid ? true : false;
                }
            }
        }

        public bool KorpaNotEmpty
        {
            get { return KorpaList != null && KorpaList.Count > 0; }
        }
        public bool KorpaValid
        {
            get { return KorpaNotEmpty && HasValidAdres; }
        }


        public string _ukupnaCijena { get; set; }
        public string UkupnaCijenaProp
        {
            get { return _ukupnaCijena;  }
            set { _ukupnaCijena = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("UkupnaCijenaProp")); }
        }

        public string _korpaPageInfo { get; set; }
        public string KorpaPageInfoProp
        {
            get { return _korpaPageInfo; }
            set { _korpaPageInfo = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("KorpaPageInfoProp")); }
        }

        public int UkupnoStavki {
            get
            {
                int u = 0;
                foreach (KorpaListItem item in KorpaList)
                {
                    u += item.Kolicina;
                }
                return u;
            }
        }

        public ICommand IsprazniKorpuCommand { get; set; }
        private void HandleIsprazniKorpu()
        {
            KorpaList.Clear();
            Global.Narudzba.StavkeNarudzbe.Clear();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("KorpaValid"));

            UkupnaCijenaProp = "Total: - KM";
            KorpaPageInfoProp = "Korpa je prazna. Pregledajte restorane i jelovnike, izaberite i dodajte nešto u korpu!";
        }


        public KorpaVM(Narudzba narudzba = null)
        {
            // BUILD COMMANDS & HANDLERS
            IsprazniKorpuCommand = new Command(HandleIsprazniKorpu);

            // BUILD LIST AND UI ELEMENTS
            Adresa = Global.GetNarucilacAdresa;
            AdresaBlokID = Global.GetNarucilacBlokID;

            if (narudzba != null && narudzba.StavkeNarudzbe != null && narudzba.StavkeNarudzbe.Count != 0)
            {
                KorpaList = new ObservableCollection<KorpaListItem>(narudzba.StavkeNarudzbe
                   .Select(s => new KorpaListItem
                   {
                       Naziv = s.Hrana.Naziv,
                       NarucenoIzRestorana = s.Hrana.RestoranNaziv,
                       Kolicina = s.Kolicina,
                       Slika = s.Hrana.Slika,
                       Cijena = s.Hrana.Cijena
                   })
                   .ToList());
                KorpaPageInfoProp = String.Format("Ukupno stavki u korpi: {0}", UkupnoStavki);
                UkupnaCijenaProp = String.Format("Total: {0} KM", KorpaList.Select(i => i.Kolicina * i.Cijena).Sum());
            }
            else
            {
                KorpaPageInfoProp = "Korpa je prazna. Pregledajte restorane i jelovnike, izaberite i dodajte nešto u korpu!";
                UkupnaCijenaProp = "Total: - KM";
            }
        }
    }
}
