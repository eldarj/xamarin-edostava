using Easyfood_Xamarin.Model;
using Easyfood_Xamarin.Util;
using Easyfood_Xamarin.Helpers;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Easyfood_Xamarin
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Login : ContentPage
	{
        private WebApiHelper servis = new WebApiHelper("auth");

		public Login ()
		{
			InitializeComponent ();
		}

        private async void BtnLogin_Clicked(object sender, EventArgs e)
        {
            loaderIndicator.IsRunning = true;
            Narucilac narucilacObj = await Task<Narucilac>.Run(() =>
            {
                HttpResponseMessage response = servis.GetResponse(inputUsername.Text).Result;
                if (response.IsSuccessStatusCode)
                {
                    string salt = JsonConvert.DeserializeObject<string>(response.Content.ReadAsStringAsync().Result);
                    Narucilac n = new Narucilac
                    {
                        Username = inputUsername.Text,
                        LozinkaHash = UIHelper.GenerateHash(inputPassword.Text, salt)
                    };

                    HttpResponseMessage loginResponse = servis.PostResponse(n).Result;
                    if (loginResponse.IsSuccessStatusCode)
                    {
                        return JsonConvert.DeserializeObject<Narucilac>(loginResponse.Content.ReadAsStringAsync().Result);
                    }
                }
                return new Narucilac();
            });

            loaderIndicator.IsRunning = false;
            if (narucilacObj != null)
            {
                Global.Narucilac = narucilacObj;
                DisplayAlert("Uspjeh", "Uspješan login!", "OK");
                Navigation.PushAsync(new RestoraniPage());
            }
            else
            {
                await DisplayAlert("Neuspješan login", "Pogrešan username ili password.", "OK");
            }
        }

        private void BtnRegistracija_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Registracija());
        }
    }
}