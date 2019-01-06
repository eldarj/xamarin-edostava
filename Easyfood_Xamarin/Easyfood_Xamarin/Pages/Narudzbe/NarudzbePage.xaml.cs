using Easyfood_Xamarin.Helpers;
using Easyfood_Xamarin.Model;
using Easyfood_Xamarin.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Easyfood_Xamarin
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NarudzbePage : ContentPage
    {
        private WebApiHelper servis = new WebApiHelper("narudzbe");
        private List<Narudzba> listNarudzbe, originalListNarudzbe;

        public NarudzbePage ()
		{
			InitializeComponent ();
		}

        protected override void OnAppearing()
        {
            if (listNarudzbe == null) // možda već imamo podatke (npr. back-button)
            {
                LoadDataFromApi();
            }
            base.OnAppearing();
        }

        private async void LoadDataFromApi()
        {
            containerApiError.IsVisible = false;
            loaderIndicator.IsRunning = loaderIndicator.IsVisible = true;
            try
            {
                HttpResponseMessage response = await servis.GetResponse(new Dictionary<string, int>{{ "narucilac", Global.Narucilac.KorisnikID }});
                if (response.IsSuccessStatusCode)
                {
                    listNarudzbe = originalListNarudzbe = JsonConvert.DeserializeObject<List<Narudzba>>(response.Content.ReadAsStringAsync().Result);
                    listViewNarudzbe.ItemsSource = listNarudzbe;
                }
            }
            catch (Exception ex)
            {
                containerApiError.IsVisible = true;
            }
            loaderIndicator.IsRunning = loaderIndicator.IsVisible = false;
        }

        private void BtnReloadPodatke_Clicked(object sender, EventArgs e)
        {
            LoadDataFromApi();
        }
        private void ListViewNarudzbe_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item != null)
            {
                Navigation.PushAsync(new NarudzbeDetailsPage(((Narudzba)e.Item).NarudzbaID));
            }
        }

        private void SearchPretraga_TextChanged(object sender, TextChangedEventArgs e)
        {
            string pretraga = searchPretraga.Text;
            var filteredList = originalListNarudzbe
                .Where(n => (n.SifraString + " " + n.StatusNarudzbe + " " + n.UkupnaCijenaFormatted + " " + n.DatumVrijemeFormatted + " " + n.AdresaBlokGrad)
                .ToLower().Contains(pretraga.ToLower()))
                .ToList();

            // Ako su liste iste (elementi u listama) nemoj džaba mijenjat listview source
            if (!listNarudzbe.SequenceEqual(filteredList))
            {
                listNarudzbe = filteredList;

                listViewNarudzbe.ItemsSource = null;
                listViewNarudzbe.ItemsSource = listNarudzbe;

                lblEmptyList.IsVisible = filteredList.Count == 0 ? true : false;
            }
        }
    }
}