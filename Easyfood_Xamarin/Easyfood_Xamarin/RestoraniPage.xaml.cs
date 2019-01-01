using Easyfood_Xamarin.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Easyfood_Xamarin.Model;
using Newtonsoft.Json;
using Xamarin.Essentials;

namespace Easyfood_Xamarin
{
    public partial class RestoraniPage : ContentPage
    {
        private WebApiHelper servis = new WebApiHelper();
        private List<Grad> gradovi;
        private List<Restoran> listRestorani;
        public RestoraniPage()
        {
            InitializeComponent();
            var mainDisplay = DeviceDisplay.MainDisplayInfo;
            var displayWidth = mainDisplay.Width;
        }

        protected async override void OnAppearing()
        {
            HttpResponseMessage response = await servis.GetResponse("locations/gradovi");
            if (response.IsSuccessStatusCode)
            {
                gradovi = JsonConvert.DeserializeObject<List<Grad>>(response.Content.ReadAsStringAsync().Result);
                pickerGradovi.ItemsSource = gradovi;
                pickerGradovi.ItemDisplayBinding = new Binding("Naziv");
            }
            HttpResponseMessage responseRestorani = await servis.GetResponse("restorani");
            if (responseRestorani.IsSuccessStatusCode)
            {
                listRestorani = JsonConvert.DeserializeObject<List<Restoran>>(responseRestorani.Content.ReadAsStringAsync().Result);
                listViewRestorani.ItemsSource = listRestorani;
            }

            base.OnAppearing();
        }
        
        private void PickerGradovi_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (pickerGradovi.SelectedItem != null)
            {
                Grad selected = pickerGradovi.SelectedItem as Grad;
                List<Restoran> temp = listRestorani.Where(r => r.GradID == selected.GradID).ToList();
                if (temp.Count == 0)
                {
                    temp.Add(new Restoran{ Naziv = "Nema restorana za traženi grad" });
                }
                listViewRestorani.ItemsSource = null;
                listViewRestorani.ItemsSource = temp;
            }
        }

        private void ListViewRestorani_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item != null)
            {
                Navigation.PushAsync(new TestVM(e.Item as Restoran));
            }
        }
    }
}
