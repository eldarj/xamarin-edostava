using Easyfood_Xamarin.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Xamarin.Forms;
using Easyfood_Xamarin.Model;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Easyfood_Xamarin.Helpers;

namespace Easyfood_Xamarin
{
    public partial class RestoraniPage : ContentPage
    {
        private WebApiHelper servisRestorani = new WebApiHelper("restorani");
        private WebApiHelper servis = new WebApiHelper();
        private List<Restoran> listRestorani;

        public RestoraniPage()
        {
            InitializeComponent();
            var mainDisplay = DeviceDisplay.MainDisplayInfo;
            var displayWidth = mainDisplay.Width;

            // TODO: xamarin.Essentials - provjeri veličinu ekrana za u zavisnosti od tog podesi layout
        }

        protected override void OnAppearing()
        {
            if (Global.Narucilac == null)
            {
                requiredLoginPartialView.IsVisible = true;
            }

            if (listRestorani == null) // možda već imamo podatke (npr. back-button)
            {
                loadDataFromApi();
            }

            base.OnAppearing();
        }

        private async void loadDataFromApi()
        {
            containerApiError.IsVisible = false;
            loaderIndicator.IsRunning = loaderIndicator.IsVisible = true;
            try
            {
                HttpResponseMessage response = await servis.GetResponse("locations/gradovi");
                if (response.IsSuccessStatusCode)
                {
                    List<Grad> gradovi = JsonConvert.DeserializeObject<List<Grad>>(response.Content.ReadAsStringAsync().Result);
                    gradovi.Insert(0, new Grad
                    {
                        Naziv = "Restorani iz svih gradova"
                    });
                    pickerGradovi.ItemsSource = gradovi;
                    pickerGradovi.ItemDisplayBinding = new Binding("Naziv");
                    pickerGradovi.SelectedIndex = 0;
                }

                HttpResponseMessage responseRestorani = await servisRestorani.GetResponse(new Dictionary<string, int> { { "status", 3 } });
                if (responseRestorani.IsSuccessStatusCode)
                {
                    listRestorani = JsonConvert.DeserializeObject<List<Restoran>>(responseRestorani.Content.ReadAsStringAsync().Result);
                    listViewRestorani.ItemsSource = listRestorani;
                } else
                {
                    containerApiError.IsVisible = true;
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
            loadDataFromApi();
        }

        private void PickerGradovi_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Grad selected = pickerGradovi.SelectedItem as Grad;
                List<Restoran> restoraniPoGradu = selected.GradID == 0 ? listRestorani : listRestorani.Where(r => r.GradID == selected.GradID).ToList();
                if (restoraniPoGradu.Count == 0)
                {
                    restoraniPoGradu.Add(new Restoran{ Naziv = "Nema restorana", Opis = "Molimo odaberite drugi grad" });
                }
                listViewRestorani.ItemsSource = null;
                listViewRestorani.ItemsSource = restoraniPoGradu;
            }
            catch (NullReferenceException ex)
            {
                // još nije setovan...
            }
        }

        private void ListViewRestorani_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item != null)
            {
                Navigation.PushAsync(new RestoraniDetailsPage(e.Item as Restoran));
            }
        }
    }
}
