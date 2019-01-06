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
using System.Collections.Generic;

namespace Easyfood_Xamarin
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Login : ContentPage
	{
        private WebApiHelper servis = new WebApiHelper();
        private string loginSalt;
        private string loginPasswordHash;


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
                    HttpResponseMessage response = servis.GetResponse("auth/" + inputUsername.Text).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        loginSalt = JsonConvert.DeserializeObject<string>(response.Content.ReadAsStringAsync().Result);
                        loginPasswordHash = UIHelper.GenerateHash(inputPassword.Text, loginSalt);
                        Narucilac n = new Narucilac
                        {
                            Username = inputUsername.Text,
                            LozinkaHash = loginPasswordHash
                        };

                        HttpResponseMessage loginResponse = servis.PostResponse("auth", n).Result;
                        if (loginResponse.IsSuccessStatusCode)
                        {
                            return JsonConvert.DeserializeObject<Narucilac>(loginResponse.Content.ReadAsStringAsync().Result);
                        }
                    }
                    return null;
                }
                catch (Exception)
                {
                    return null;
                }
            });

            loaderIndicator.IsRunning = false;
            if (narucilacObj != null)
            {
                HttpResponseMessage response = await servis.GetResponse("korisnici/" + narucilacObj.KorisnikID + "/omiljenirestorani");
                if (response.IsSuccessStatusCode)
                    Global.CurrentOmiljeniRestorani = JsonConvert.DeserializeObject<List<Restoran>>(response.Content.ReadAsStringAsync().Result);

                DisplayAlert("Uspjeh", "Uspješno ste se ulogirali!", "Ok");
                Global.Narucilac = narucilacObj;
                Global.Narucilac.LozinkaHash = loginPasswordHash;
                Global.Narucilac.LozinkaSalt = loginSalt;

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