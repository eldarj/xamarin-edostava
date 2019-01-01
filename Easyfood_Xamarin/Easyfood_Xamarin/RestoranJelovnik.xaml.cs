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
	public partial class TestVM : ContentPage
	{
        private WebApiHelper servis = new WebApiHelper();

        private List<Hrana> listfromapi;
        private Restoran restoran;
        private HranaVM vm = new HranaVM();

		public TestVM ()
		{
			InitializeComponent ();
            BindingContext = vm;
        }
        public TestVM(Restoran restoran)
        {
            InitializeComponent();
            BindingContext = vm; // set binding context to our ViewModel
            this.restoran = restoran;
        }

        protected async override void OnAppearing()
        {
            HttpResponseMessage response = await servis.GetResponse(String.Format("restorani/{0}/hrana", restoran.RestoranID));
            if (response.IsSuccessStatusCode)
            {
                listfromapi = JsonConvert.DeserializeObject<List<Hrana>>(response.Content.ReadAsStringAsync().Result);
                vm.SetListHrana(listfromapi);
                HranaListItems.ItemsSource = vm.listHrana;
            }

            base.OnAppearing();
        }

        private void SearchPretraga_TextChanged(object sender, TextChangedEventArgs e)
        {
            string pretraga = searchPretraga.Text;
            vm.SetListHrana(listfromapi.Where(h => (h.Naziv + " " + h.TipKuhinjeNaziv + " " + h.Opis + " " + h.Cijena).ToLower().Contains(pretraga.ToLower())).ToList());
            HranaListItems.ItemsSource = null;
            HranaListItems.ItemsSource = vm.listHrana;
        }
    }
}