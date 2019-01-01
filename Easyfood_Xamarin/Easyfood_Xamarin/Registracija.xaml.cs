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

namespace Easyfood_Xamarin
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Registracija : ContentPage
	{
        private WebApiHelper servis = new WebApiHelper("auth");

		public Registracija ()
		{
			InitializeComponent ();
            this.Title = "Registracija";
        }

        private void Registracija_Clicked(object sender, EventArgs e)
        {
            Narucilac n = new Narucilac
            {
                Ime = inputIme.Text,
                Prezime = inputPrezime.Text,
                Username = inputUsername.Text
            };

            try
            {
                HttpResponseMessage response = servis.PostResponse("register", n).Result;
                if (response.IsSuccessStatusCode)
                {
                    DisplayAlert("Uspjeh", "Uspješno ste se registrovali", "OK");
                    Navigation.PushAsync(new RestoraniPage());
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