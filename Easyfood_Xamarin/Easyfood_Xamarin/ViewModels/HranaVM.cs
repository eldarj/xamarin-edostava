using Easyfood_Xamarin.Model;
using Easyfood_Xamarin.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Easyfood_Xamarin.ViewModels
{
    public class HranaVM
    {
        public class HranaListItem : INotifyPropertyChanged
        {
            public Hrana hranaStavka { get; set; }
            public byte[] Slika { get; set; }
            public string TipKuhinjeNaziv { get; set; }
            public string Opis { get; set; }
            public double Cijena { get; set; }
            public int Kolicina { get; set; } = 0;

            
            public event PropertyChangedEventHandler PropertyChanged;

            #region BindingProps
            private string _stanjeUKorpi { get; set; } = "";
            public string StanjeUKorpiProp
            {
                get { return _stanjeUKorpi; }
                set { _stanjeUKorpi = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("StanjeUKorpiProp")); }
            }

            private bool _stanjeUKorpiVisibility = false;
            public bool StanjeUKorpiVisibility  {
                get { return _stanjeUKorpiVisibility; }
                set { _stanjeUKorpiVisibility = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("StanjeUKorpiVisibility")); }
            }

            private string _hranaAddedButtonImg = "add.png";
            public string HranaAddedButtonImg 
            {
                get { return _hranaAddedButtonImg; }
                set { _hranaAddedButtonImg = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("HranaAddedButtonImg")); }
            }

            private string _hranaRemoveButtonImg = "minus.png";
            public string HranaRemoveButtonImg
            {
                get { return _hranaRemoveButtonImg; }
                set { _hranaRemoveButtonImg = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("HranaRemoveButtonImg")); }
            }
            #endregion

            #region Commands
            public ICommand HranaAddedCommand { get; set; }
            private void HandleHranaAdded()
            {
                Kolicina++;
                StanjeUKorpiProp = String.Format("Stanje u korpi: x{0} - ukupno {1} KM", Kolicina, Kolicina * Cijena);
                StanjeUKorpiVisibility = true;
                HranaAddedButtonImg = "add_red.png";
                HranaRemoveButtonImg = "minus_red.png";

                if (Global.Narudzba == null)
                {
                    Global.Narudzba = new Narudzba
                    {
                        Adresa = Global.GetNarucilacAdresa,
                        BlokID = Global.Narucilac?.BlokID,
                        NarucilacID = Global.Narucilac?.KorisnikID,
                        Sifra = Guid.NewGuid(),
                        UkupnaCijena = 0
                    };
                }
                Global.AddHrana(hranaStavka);
            }
            public ICommand HranaRemovedCommand { get; set; }
            private void HandleHranaRemoved()
            {
                Kolicina--;
                if (Kolicina < 1)
                {
                    Kolicina = 0;
                    StanjeUKorpiVisibility = false;
                    HranaAddedButtonImg = "add.png";
                    HranaRemoveButtonImg = "minus.png";
                }
                else
                {
                    StanjeUKorpiProp = String.Format("Stanje u korpi: x{0} - ukupno {1} KM", Kolicina, Kolicina * Cijena);
                }

                if (Global.Narudzba != null)
                {
                    Global.RemoveHrana(hranaStavka);
                }
            }
            #endregion

            public HranaListItem()
            {
                HranaAddedCommand = new Command(HandleHranaAdded);
                HranaRemovedCommand = new Command(HandleHranaRemoved);
            }
        }

        public List<HranaListItem> listHrana;

        public void SetListHrana(List<Hrana> list)
        {
            this.listHrana = new List<HranaListItem>(list
                .Select(s => {

                    HranaListItem sItem = new HranaListItem
                    {
                        hranaStavka = s,
                        TipKuhinjeNaziv = s.TipKuhinjeNaziv,
                        Opis = s.Opis,
                        Slika = s.Slika,
                        Cijena = s.Cijena
                    };

                    StavkaNarudzbe inKorpa = null;
                    if (Global.Narudzba != null && Global.Narudzba.StavkeNarudzbe.Where(h => h.HranaID == s.HranaID).SingleOrDefault() != null)
                    {
                        inKorpa = Global.Narudzba.StavkeNarudzbe.Where(h => h.HranaID == s.HranaID).SingleOrDefault();
                        sItem.Kolicina = inKorpa.Kolicina;
                        sItem.StanjeUKorpiProp = String.Format("Stanje u korpi: x{0} - ukupno {1} KM", sItem.Kolicina, sItem.Cijena);
                        sItem.StanjeUKorpiVisibility = true;
                        sItem.HranaAddedButtonImg = "add_red.png";
                        sItem.HranaRemoveButtonImg = "minus_red.png";
                    }

                    return sItem;
                })
                .ToList());
        }
    }
}
