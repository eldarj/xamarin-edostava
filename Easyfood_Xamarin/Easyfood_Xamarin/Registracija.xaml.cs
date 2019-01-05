using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Easyfood_Xamarin.Util;
using System.Net.Http;
using Newtonsoft.Json;
using Easyfood_Xamarin.Model;
using Easyfood_Xamarin.Helpers;
using Easyfood_PCL.Model;
using Easyfood_Xamarin.Validators;

namespace Easyfood_Xamarin
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Registracija : ContentPage
	{
        private WebApiHelper servis = new WebApiHelper();
        private List<Blok> listBlokovi;
        private List<Grad> listGradovi;

        private NarucilacValidator validacija;

        public Registracija ()
		{
			InitializeComponent ();
            this.Title = "Registracija";
        }

        protected override void OnAppearing()
        {
            LoadDataFromApi();

            if (Global.Narucilac != null)
            {
                inputAdresa.Text = Global.Narucilac.Adresa;
            }

            base.OnAppearing();
        }

        private void BtnReloadPodatke_Clicked(object sender, EventArgs e)
        {
            LoadDataFromApi();
        }

        private async void LoadDataFromApi()
        {
            containerApiError.IsVisible = false;
            loaderIndicator.IsRunning = true;
            try
            {
                HttpResponseMessage responseBlokovi = await servis.GetResponse("locations/blokovi");
                if (responseBlokovi.IsSuccessStatusCode)
                {
                    listBlokovi = JsonConvert.DeserializeObject<List<Blok>>(responseBlokovi.Content.ReadAsStringAsync().Result);

                    pickerLokacija.ItemsSource = listBlokovi;
                    pickerLokacija.ItemDisplayBinding = new Binding("BlogGradFull");
                    if (Global.Narucilac != null)
                    {
                        pickerLokacija.SelectedItem = listBlokovi.Where(b => b.BlokID == Global.Narucilac.BlokID).Single();
                    }
                    else
                    {
                        pickerLokacija.SelectedIndex = 0;
                    }
                }
                else
                {
                    containerApiError.IsVisible = true;
                }
            } catch (Exception ex)
            {
                containerApiError.IsVisible = true;
            }
            loaderIndicator.IsRunning = false;
        }

        private async void Registracija_Clicked(object sender, EventArgs e)
        {
            // provjeri formu
            validacija = new NarucilacValidator(ime: inputIme.Text, 
                    username: inputUsername.Text, 
                    password: inputPassword.Text, 
                    prezime: inputPrezime.Text, 
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

            string salt = UIHelper.GenerateSalt();
            Narucilac n = new Narucilac
            {
                Ime = inputIme.Text,
                Prezime = inputPrezime.Text,
                Username = inputUsername.Text,
                Adresa = inputAdresa.Text,
                BlokID = ((Blok)pickerLokacija.SelectedItem).BlokID,
                LozinkaSalt = salt,
                LozinkaHash = UIHelper.GenerateHash(inputPassword.Text, salt)
            };

            try
            {
                HttpResponseMessage response = await servis.PostResponse("auth/register", n);
                if (response.IsSuccessStatusCode)
                {
                    DisplayAlert("Uspjeh", "Uspješno ste se registrovali!", "OK");
                    if (Global.OnUserAuthGoToKorpaFlag)
                    {
                        Navigation.PushAsync(new KorpaPage());
                    }
                    else
                    {
                        Navigation.PushAsync(new RestoraniPage());
                    }
                }
                else
                {
                    DisplayAlert("Neuspjeh", "Username je već zauzet.", "OK");
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Neuspjeh", "Greška sa servisom, pokušajte ponovo", "OK");
            }
        }
    }
}