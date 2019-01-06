using Easyfood_PCL.Model;
using Easyfood_Xamarin.Helpers;
using Easyfood_Xamarin.Model;
using Easyfood_Xamarin.Util;
using Easyfood_Xamarin.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static Easyfood_Xamarin.ViewModels.RestoranDetailsVM;

namespace Easyfood_Xamarin
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RestoraniDetailsPage : ContentPage
	{
        private RestoranDetailsVM vm;
        private Restoran restoran;

        public RestoraniDetailsPage (Restoran r)
		{
			InitializeComponent ();

            restoran = r;
            vm = new RestoranDetailsVM(r);
            vm.Navigation = Navigation;

            BindingContext = vm;
		}

        protected override void OnAppearing()
        {
            if (Global.Narucilac != null)
            {
                requiredLoginPartialView.IsVisible = false;
                BtnDodajUOmiljene.IsVisible = true;
            }

            GetRecenzijeFromApi();

            base.OnAppearing();
        }

        public async void GetRecenzijeFromApi()
        {
            vm.ListRecenzije = new ObservableCollection<RecenzijaItem>() {
                new RecenzijaItem(ime: Global.Narucilac != null ? "Budite prvi što će napisati komentar!" : "Nema komentara.")
            };

            HttpResponseMessage response = await new WebApiHelper("recenzije").GetResponse(new Dictionary<string, int> { { "restoran", restoran.RestoranID } });
            if (response.IsSuccessStatusCode)
            {
                List<Recenzija> responseitems = JsonConvert.DeserializeObject<List<Recenzija>>(response.Content.ReadAsStringAsync().Result)
                    .OrderByDescending(x => x.Datum)
                    .ToList();

                if (responseitems.Count > 0)
                    vm.ListRecenzije = new ObservableCollection<RecenzijaItem>(responseitems.Select(x => new RecenzijaItem(x)));

                vm.UkupnoRecenzija = String.Format("Ukupno recenzija: {0}", vm.ListRecenzije.Count);
                vm.ProsjecnaOcjena = vm.ListRecenzije.Average(r => r.Ocjena);
            }
        }

        private void BtnGotoRecenzije_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new RestoraniRecenzijePage(vm));
        }

        private void BtnGotoJelovnik_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new RestoranJelovnikPage(vm.RestoranID));
        }

        private void BtnGotoSlicniRestorani_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new RestoraniSlicniPage(restoran));
        }
    }
}