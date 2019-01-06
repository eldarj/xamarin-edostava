using Easyfood_PCL.Model;
using Easyfood_Xamarin.Helpers;
using Easyfood_Xamarin.Model;
using Easyfood_Xamarin.Util;
using Easyfood_Xamarin.Validators;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Easyfood_Xamarin.Pages.Profil
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ProfilEditPage : ContentPage
    {
        private WebApiHelper servis = new WebApiHelper();
        private List<Blok> listBlokovi;
        private NarucilacValidator validacija;

        public ProfilEditPage ()
		{
            BindingContext = Global.Narucilac;
            InitializeComponent ();
		}

        protected override void OnAppearing()
        {
            LoadDataFromApi();
            base.OnAppearing();
        }

        private async void LoadDataFromApi()
        {
            containerApiError.IsVisible = false;
            loaderIndicator.IsRunning = loaderIndicator.IsVisible = true;
            try
            {
                HttpResponseMessage responseBlokovi = await servis.GetResponse("locations/blokovi");
                if (responseBlokovi.IsSuccessStatusCode)
                {
                    BtnUpdate.IsEnabled = true;
                    listBlokovi = JsonConvert.DeserializeObject<List<Blok>>(responseBlokovi.Content.ReadAsStringAsync().Result);

                    pickerLokacija.ItemsSource = listBlokovi;
                    pickerLokacija.ItemDisplayBinding = new Binding("BlogGradFull");
                    pickerLokacija.SelectedItem = listBlokovi.Where(b => b.BlokID == Global.Narucilac.BlokID).Single();
                }
                else
                {
                    containerApiError.IsVisible = true;
                }
            }
            catch (Exception ex)
            {
                containerApiError.IsVisible = true;
            }
            loaderIndicator.IsRunning = loaderIndicator.IsVisible  = false;
        }
        private async void BtnUpdate_Clicked(object sender, EventArgs e)
        {
            // provjeri formu
            validacija = new NarucilacValidator(ime: inputIme.Text,
                    prezime: inputPrezime.Text,
                    email: inputEmail.Text,
                    telefon: inputTelefon.Text,
                    adresa: inputAdresa.Text);

            if (!validacija.IsValid)
            {
                foreach (ValidatorField field in validacija.ValidatedFields)
                {
                    Label lblError = (Label)this.FindByName(field.PropName);
                    if (lblError != null)
                    {
                        lblError.IsVisible = false; // (re)set visibility
                        lblError.Text = field.ErrorMsg;
                        lblError.IsVisible = !field.IsValid;
                    }
                }
                return;
            }

            Narucilac n = new Narucilac
            {
                KorisnikID = Global.Narucilac.KorisnikID,
                Ime = inputIme.Text,
                Prezime = inputPrezime.Text,
                Adresa = inputAdresa.Text,
                BlokID = ((Blok)pickerLokacija.SelectedItem).BlokID,
                Telefon = inputTelefon.Text,
                Email = inputEmail.Text
            };

            try
            {
                HttpResponseMessage response = await servis.PutResponse("auth/update", n);
                if (response.IsSuccessStatusCode)
                {
                    DisplayAlert("Uspjeh", "Uspješno ste uredili podatke!", "OK");

                    Global.Narucilac = JsonConvert.DeserializeObject<Narucilac>(response.Content.ReadAsStringAsync().Result);
                    App.Current.MainPage = new Navigation.MyMasterDetailPage(new ProfilPage());
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Neuspjeh", "Greška sa servisom, pokušajte ponovo", "OK");
            }
        }

        private void BtnOdustani_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private void BtnReloadPodatke_Clicked(object sender, EventArgs e)
        {
            LoadDataFromApi();
        }
    }
}