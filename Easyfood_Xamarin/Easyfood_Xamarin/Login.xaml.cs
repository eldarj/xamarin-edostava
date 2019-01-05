using Easyfood_Xamarin.Model;
using Easyfood_Xamarin.Util;
using Easyfood_Xamarin.Helpers;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Easyfood_Xamarin.Validators;

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
            // validate form
            var validacija = new NarucilacValidator(username: inputUsername.Text, password: inputPassword.Text);
            if (!validacija.IsValid)
            {
                foreach (var v in validacija.ValidatedFields)
                {
                    Label lblError = (Label)this.FindByName(v.PropName);
                    if (lblError != null)
                    {
                        lblError.IsVisible = false; // (re)set visibility
                        lblError.Text = v.ErrorMsg;
                        lblError.IsVisible = !v.IsValid;
                    }
                }
                return;
            }

            loaderIndicator.IsRunning = true;
            Narucilac narucilacObj = await Task<Narucilac>.Run(() =>
            {
                try
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
                }
                catch (Exception)
                {
                    return null;
                }
            });

            loaderIndicator.IsRunning = false;
            if (narucilacObj != null)
            {
                DisplayAlert("Uspjeh", "Uspješno ste se ulogirali!", "OK");
                Global.Narucilac = narucilacObj;

                App.Current.MainPage = Global.OnUserAuthGoToKorpaFlag  ? 
                    new Navigation.MyMasterDetailPage(new KorpaPage()) :
                    new Navigation.MyMasterDetailPage();
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