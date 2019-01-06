using Easyfood_PCL.Model;
using Easyfood_Xamarin.Helpers;
using Easyfood_Xamarin.Model;
using Easyfood_Xamarin.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Easyfood_Xamarin.ViewModels
{
    public class RestoranDetailsVM : INotifyPropertyChanged
    {
        public class RecenzijaItem
        {
            private DateTime _datum { get; set; }
            private string _ime { get; set; }
            private string _prezime { get; set; }

            public int Ocjena { get; set; }
            public string OcjenaStr { get { return "Ocjena(" + Ocjena + ")"; } }
            public string Komentar { get; set; }
            public string ImePrezime => _ime + " " + _prezime;
            public string Datum => _datum.ToString("dd.MM.yyyy");

            public RecenzijaItem(string ime) {
                _ime = ime;
            }

            public RecenzijaItem(Recenzija recenzija) {
                _datum = recenzija.Datum;
                _ime = recenzija.Ime;
                _prezime = recenzija.Prezime;
                Komentar = recenzija.Komentar;
                Ocjena = recenzija.Ocjena;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public INavigation Navigation { get; set; }
        public int RestoranID { get; set; }

        #region BinderPropsAndNotifiers
        public byte[] Slika { get; set; }
        public string Naziv { get; set; }
        public string Opis { get; set; }
        public string Email { get; set; }
        public string WebUrl { get; set; }
        public string AdresaFull { get; set; }
        public string Telefon { get; set; }


        public ObservableCollection<RecenzijaItem> _listRecenzije { get; set; }
        public ObservableCollection<RecenzijaItem> ListRecenzije {
            get { return _listRecenzije; }
            set { _listRecenzije = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ListRecenzije")); }
        }

        private bool _isOmiljeniRestoran { get; set; } = false;
        public bool IsOmiljeniRestoran
        {
            get { return _isOmiljeniRestoran; }
            set {
                _isOmiljeniRestoran = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsOmiljeni"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsOmiljeniImgSource"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("OmiljeniStatusString"));
            }
        }
        public string IsOmiljeniImgSource => _isOmiljeniRestoran ? "heart_red.png" : "heart_dark.png";
        public string OmiljeniStatusString { get; set; } = "";
        private bool _noviKomentarError { get; set; } = false;
        public bool NoviKomentarError
        {
            get { return _noviKomentarError; }
            set { _noviKomentarError = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("NoviKomentarError")); }
        }
        private bool _noviKomentarSuccess { get; set; } = false;
        public bool NoviKomentarSuccess
        {
            get { return _noviKomentarSuccess; }
            set { _noviKomentarSuccess = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("NoviKomentarSuccess")); }
        }
        private int _novaRecenzijaOcjena { get; set; }
        public int NovaRecenzijaOcjena {
            get { return _novaRecenzijaOcjena; }
            set { _novaRecenzijaOcjena = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("NovaRecenzijaOcjena"));  }
        }

        private string _ukupnoRecenzija { get; set; }
        public string UkupnoRecenzija
        {
            get { return _ukupnoRecenzija; }
            set { _ukupnoRecenzija = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("UkupnoRecenzija")); }
        }
        private double _prosjecnaOcjena { get; set; }
        public double ProsjecnaOcjena
        {
            get { return _prosjecnaOcjena; }
            set { _prosjecnaOcjena = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ProsjecnaOcjena")); }
        }
        #endregion

        #region Commands
        public ICommand NovaOcjenaCommand { get; set; }
        private void HandleOcjenaCommand(string clickedVal)
        {
            if (NovaRecenzijaOcjena != Convert.ToInt32(clickedVal)) NovaRecenzijaOcjena = Convert.ToInt32(clickedVal);
        }

        public ICommand ToggleOmiljeni { get; set; }
        private async void HandleToggleOmiljeni()
        {
            HttpResponseMessage response = await new WebApiHelper("korisnici").PostResponse(Global.Narucilac.KorisnikID + "/omiljenirestorani", RestoranID);
            if (response.IsSuccessStatusCode)
            {
                IsOmiljeniRestoran = !IsOmiljeniRestoran;
                OmiljeniStatusString = IsOmiljeniRestoran ? "Dodali restoran u omiljene!" : "Uklonili ste restoran iz omiljenih!";
            }
        }

        public ICommand NoviKomentarCommand;
        private async void HandleNoviKomentar(string komentartext)
        {
            if (komentartext.Length < 5)
            {
                NoviKomentarError = true;
                return;
            }
            NoviKomentarError = false;

            var novaRec = new Recenzija()
            {
                KorisnikID = Global.Narucilac.KorisnikID,
                Datum = DateTime.Now,
                RestoranID = RestoranID,
                Komentar = komentartext,
                Ocjena = NovaRecenzijaOcjena
            };
            HttpResponseMessage response = await new WebApiHelper("recenzije").PostResponse(novaRec);

            if (response.IsSuccessStatusCode)
            {
                List<Recenzija> responseitems = JsonConvert.DeserializeObject<List<Recenzija>>(response.Content.ReadAsStringAsync().Result)
                    .OrderByDescending(x => x.Datum)
                    .ToList();

                if (responseitems.Count > 0)
                {
                    ListRecenzije = new ObservableCollection<RecenzijaItem>(responseitems.Select(x => new RecenzijaItem(x)));
                    UkupnoRecenzija = String.Format("Ukupno recenzija: {0}", ListRecenzije.Count);
                    ProsjecnaOcjena = ListRecenzije.Average(r => r.Ocjena);
                    NoviKomentarSuccess = true;
                }
            }
        }
        #endregion

        public RestoranDetailsVM(Restoran r)
        {
            RestoranID = r.RestoranID;
            Naziv = r.Naziv;
            Slika = r.Slika;
            Opis = r.Opis;
            Telefon = r.Telefon;
            Email = r.Email;
            WebUrl = r.WebUrl;
            AdresaFull = r.AdresaFull;
            IsOmiljeniRestoran = Global.CurrentOmiljeniRestorani.Select(o => o.RestoranID).Contains(RestoranID);

            ToggleOmiljeni = new Command(HandleToggleOmiljeni);
            NovaOcjenaCommand = new Command<string>(HandleOcjenaCommand);
            NoviKomentarCommand = new Command<string>(HandleNoviKomentar);
        }
    }
}
