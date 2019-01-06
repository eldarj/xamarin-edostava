using Easyfood_Xamarin.Model;
using Easyfood_Xamarin.Util;
using Easyfood_Xamarin.Helpers;
using Easyfood_Xamarin.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static Easyfood_Xamarin.ViewModels.HranaVM;

namespace Easyfood_Xamarin
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RestoranJelovnikPage : ContentPage
	{
        private WebApiHelper servis = new WebApiHelper();

        private List<Hrana> originalListHrana;
        private HranaVM vm = new HranaVM();
        private int restoranId;

        public RestoranJelovnikPage ()
		{
			InitializeComponent ();
            BindingContext = vm;
        }

        public RestoranJelovnikPage(int restoranId)
        {
            InitializeComponent();
            BindingContext = vm; // set binding context to our ViewModel
            this.restoranId = restoranId;
        }

        protected async override void OnAppearing()
        {
            if (Global.Narucilac == null)
            {
                requiredLoginPartialView.IsVisible = true;
            }

            HttpResponseMessage response = await servis.GetResponse(String.Format("restorani/{0}/hrana", restoranId));
            if (response.IsSuccessStatusCode)
            {
                originalListHrana = JsonConvert.DeserializeObject<List<Hrana>>(response.Content.ReadAsStringAsync().Result);
                vm.SetListHrana(originalListHrana);
                HranaListItems.ItemsSource = vm.listHrana;
            }
            loaderIndicator.IsRunning = loaderIndicator.IsVisible = false;
            base.OnAppearing();
        }

        private void SearchPretraga_TextChanged(object sender, TextChangedEventArgs e)
        {
            string pretraga = searchPretraga.Text;
            var filteredList = originalListHrana.Where(h => (h.Naziv + " " + h.TipKuhinjeNaziv + " " + h.Opis + " " + h.Cijena).ToLower().Contains(pretraga.ToLower())).ToList();

            /***
             * Ako su liste iste (elementi u listama) nemoj džaba mijenjat listview source 
             * (REF. POGLEDAJ NarudzbePage.SearchPretraga_TextChanged())
             * u ovom slučaju ne možemo iskoristit .SequenceEqual jer nam liste nisu isto tipa (vm.listHrana vs. templist), 
             * pa zbog tog ćemo napravit i porediti ID hashset-ove
             **/
            var vmListHash = new HashSet<int>(vm.listHrana.Select(h => h.hranaStavka.HranaID));
            var filteredListHash = new HashSet<int>(filteredList.Select(h => h.HranaID));

            if (!vmListHash.SequenceEqual(filteredListHash))
            {
                vm.SetListHrana(filteredList);
                HranaListItems.ItemsSource = null;
                HranaListItems.ItemsSource = vm.listHrana;

                lblEmptyList.IsVisible = filteredListHash.Count == 0 ? true : false;
            }
        }

        private void HranaListItems_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            // Nemoj selektovati (highlight) iteme lise
            if (e == null) return;
            ((ListView)sender).SelectedItem = null;
        }

        private void BtnGotoKorpa_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new KorpaPage());
        }
    }
}