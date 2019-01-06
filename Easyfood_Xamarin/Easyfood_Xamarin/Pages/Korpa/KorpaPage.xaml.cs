using Easyfood_PCL.Model;
using Easyfood_Xamarin.Helpers;
using Easyfood_Xamarin.Model;
using Easyfood_Xamarin.Util;
using Easyfood_Xamarin.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Easyfood_Xamarin
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class KorpaPage : ContentPage
    {
        private WebApiHelper servis = new WebApiHelper();
        private KorpaVM vm;
		public KorpaPage ()
        {
            InitializeComponent ();
            vm = new KorpaVM(Global.Narudzba)
            {
                Navigation = Navigation
            };
            BindingContext = vm;
        }

        protected async override void OnAppearing()
        {
            if (Global.Narucilac == null)
            {
                requiredLoginPartialView.IsVisible = true;
            }

            try
            {
                HttpResponseMessage response = await servis.GetResponse("locations/blokovi");
                if (response.IsSuccessStatusCode)
                {
                    List<Blok> listaBlokovaGradova = JsonConvert.DeserializeObject<List<Blok>>(response.Content.ReadAsStringAsync().Result);

                    pickerLokacija.ItemsSource = listaBlokovaGradova;
                    pickerLokacija.ItemDisplayBinding = new Binding("BlogGradFull");
                    if (vm.AdresaBlokID != null)
                    {
                        pickerLokacija.SelectedItem = listaBlokovaGradova.Where(b => b.BlokID == vm.AdresaBlokID).Single();
                    }
                    else
                    {
                        pickerLokacija.SelectedIndex = 0;
                    }
                }
            }
            catch (Exception e)
            {
                //
            }
            
            base.OnAppearing();
        }

        private async void BtnOdbaciNarudzbu_Clicked(object sender, EventArgs e)
        {
            bool choice = await DisplayAlert("Odbaci narudžbu",
                "Želite odbaciti cijelu narudžbu? Sve trenutne stavke će biti uklonjene sa vaše narudžbe.",
                 "Da, odbaci", "Ne");
            if (choice)
            {
                vm.IsprazniKorpuCommand.Execute(null);
            }
        }

        private async void BtnIzvsiNarudzbu_Clicked(object sender, EventArgs e)
        {
            if (Global.Narucilac != null && Global.Narucilac.KorisnikID != 0)
            {
                Global.Narudzba.Adresa = vm.Adresa;
                Global.Narudzba.BlokID = vm.AdresaBlokID;
                Global.Narudzba.NarucilacID = Global.Narucilac.KorisnikID;

                HttpResponseMessage response = await servis.PostResponse("narudzbe", Global.Narudzba);
                if (response.IsSuccessStatusCode)
                {
                    Narudzba n = JsonConvert.DeserializeObject<Narudzba>(response.Content.ReadAsStringAsync().Result);
                    bool idinanarudzbu = await DisplayAlert("Uspjeh", "Uspješno ste kreirali narudžbu " + n.Sifra.ToString().Substring(0, 8), "Pogledaj narudžbu", "OK");
                    vm.IsprazniKorpuCommand.Execute(null);

                    if (idinanarudzbu)
                    {
                        Navigation.PushAsync(new NarudzbeDetailsPage(n.NarudzbaID));
                    }

                } else
                {
                    DisplayAlert("Greška", "Došlo je do greške, molimo pokušajte ponovo.", "OK");
                }
            }
            else
            {
                bool choice = await DisplayAlert("Niste logirani",
                    "Da biste kreirali narudžbu morate biti registrovani. Idi na login/registraciju?",
                     "Da", "Ne, odustani");
                if (choice)
                {
                    Global.OnUserAuthGoToKorpaFlag = true;
                    Global.Narucilac = new Narucilac
                    {
                        Adresa = inputAdresa.Text,
                        BlokID = ((Blok)pickerLokacija.SelectedItem).BlokID
                    };
                    Navigation.PushAsync(new Login());
                }
            }
        }

        private void InputAdresa_TextChanged(object sender, TextChangedEventArgs e)
        {
            vm.Adresa = inputAdresa.Text;
        }
        private void BtnGotoLogin_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Login());
        }
    }
}