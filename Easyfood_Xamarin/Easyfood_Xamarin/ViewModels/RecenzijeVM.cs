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
    class RecenzijeVM : INotifyPropertyChanged
    {
        public class RecenzijaItem
        {
            private DateTime _datum { get; set; }
            private string _ime { get; set; }
            private string _prezime { get; set; }

            public int Ocjena { get; set; }
            public string Komentar { get; set; }
            public string ImePrezime => _ime + " " + _prezime;
            public string Datum => _datum.ToString("dd.MM.yyyy");

            public RecenzijaItem(string ime)
            {
                _ime = ime;
            }

            public RecenzijaItem(Recenzija recenzija)
            {
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
        public string Naziv { get; set; }
        public ObservableCollection<RecenzijaItem> _listRecenzije { get; set; }
        public ObservableCollection<RecenzijaItem> ListRecenzije {
            get { return _listRecenzije; }
            set { _listRecenzije = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ListRecenzije")); }
        }

        private bool _noviKomentarError { get; set; } = false;
        public bool NoviKomentarError
        {
            get { return _noviKomentarError; }
            set { _noviKomentarError = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("NoviKomentarError")); }
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

        public ICommand NoviKomentarCommand;
        private async void HandleNoviKomentar(string komentartext)
        {
            if (komentartext.Length < 5)
            {
                NoviKomentarError = true;
                return;
            }
            NoviKomentarError = false;

            HttpResponseMessage response = await new WebApiHelper("recenzije").PostResponse(new Recenzija(){
                    KorisnikID = Global.Narucilac.KorisnikID,
                    Datum = DateTime.Now,
                    RestoranID = RestoranID,
                    Komentar = komentartext,
                    Ocjena = NovaRecenzijaOcjena
                });

            if (response.IsSuccessStatusCode)
            {

            }
        }

        #endregion

        public RecenzijeVM(Restoran r)
        {
            Naziv = r.Naziv;
            RestoranID = r.RestoranID;

            NovaOcjenaCommand = new Command<string>(HandleOcjenaCommand);
            NoviKomentarCommand = new Command<string>(HandleNoviKomentar);

            GetRecenzijeFromApi();
        }

        public async void GetRecenzijeFromApi()
        {
            ListRecenzije = new ObservableCollection<RecenzijaItem>() {
                new RecenzijaItem(ime: Global.Narucilac != null ? "Budite prvi što će napisati komentar!" : "Nema komentara.")
            };

            HttpResponseMessage response = await new WebApiHelper("recenzije").GetResponse(new Dictionary<string, int> {{ "restoran", RestoranID }});
            if (response.IsSuccessStatusCode)
            {
                List<Recenzija> responseitems = JsonConvert.DeserializeObject<List<Recenzija>>(response.Content.ReadAsStringAsync().Result)
                    .OrderByDescending(x => x.Datum)
                    .ToList();

                if (responseitems.Count > 0)
                    ListRecenzije = new ObservableCollection<RecenzijaItem>(responseitems.Select(x => new RecenzijaItem(x)));

                UkupnoRecenzija = String.Format("Ukupno recenzija: {0}", ListRecenzije.Count);
                ProsjecnaOcjena = ListRecenzije.Average(r => r.Ocjena);
            }
        }
    }
}
