using Easyfood_Xamarin.Helpers;
using Easyfood_Xamarin.Model;
using Easyfood_PCL.Model;
using Easyfood_Xamarin.Util;
using Easyfood_Xamarin.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Net.Http;
using Newtonsoft.Json;

namespace Easyfood_Xamarin.Pages.Profil
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfilEditPasswordPage : ContentPage
    {
        private WebApiHelper servis = new WebApiHelper();
        private NarucilacValidator validacija;

        private bool isPasswordTrenutniValid;
        private bool isPasswordNoviValid;


        public ProfilEditPasswordPage()
        {
            BindingContext = Global.Narucilac;
            InitializeComponent();
        }

        private async void BtnUpdate_Clicked(object sender, EventArgs e)
        {
            // provjeri formu
            validacija = new NarucilacValidator(passwordTrenutni: inputPasswordTrenutni.Text,
                    passwordNovi: inputPasswordNovi.Text,
                    passwordNoviPonovo: inputPasswordNoviPonovo.Text);

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
                        BtnUpdate.IsEnabled = true;
                        BtnUpdate.IsEnabled = !field.IsValid;
                    }
                }
                return;
            }

            string saltNovi = UIHelper.GenerateSalt();
            string hashNovi = UIHelper.GenerateHash(inputPasswordNovi.Text, saltNovi);
            PasswordEditModel pwMod = new PasswordEditModel
            {
                KorisnikID = Global.Narucilac.KorisnikID,
                LozinkaTrenutnaHash = Global.Narucilac.LozinkaHash,
                LozinkaNovaHash = hashNovi,
                LozinkaNovaSalt = saltNovi
            };

            try
            {
                HttpResponseMessage response = await servis.PutResponse("auth/update/password", pwMod);
                if (response.IsSuccessStatusCode)
                {
                    DisplayAlert("Uspjeh", "Uspješno ste promijenili lozinku!", "OK");

                    Global.Narucilac = JsonConvert.DeserializeObject<Narucilac>(response.Content.ReadAsStringAsync().Result);
                    Global.Narucilac.LozinkaHash = hashNovi;
                    Global.Narucilac.LozinkaSalt = saltNovi;

                    App.Current.MainPage = new Navigation.MyMasterDetailPage(new ProfilPage());
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Neuspjeh", "Greška sa servisom, pokušajte ponovo", "Ok");
            }
        }
        private void BtnOdustani_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private void InputPasswordTrenutni_TextChanged(object sender, TextChangedEventArgs e)
        {
            isPasswordTrenutniValid = UIHelper.GenerateHash(inputPasswordTrenutni.Text, Global.Narucilac.LozinkaSalt) == Global.Narucilac.LozinkaHash;
            if (!isPasswordTrenutniValid)
            {
                PasswordTrenutniError.Text = "Lozinka nije tačna!";
                PasswordTrenutniError.IsVisible = true;
                BtnUpdate.IsEnabled = false;
            }
            else
            {
                PasswordTrenutniError.IsVisible = false;
                if (isPasswordNoviValid)
                {
                    BtnUpdate.IsEnabled = true;
                }
            }
        }

        private void InputPasswordNoviPonovo_TextChanged(object sender, TextChangedEventArgs e)
        {
            checkNoveLozinke();
        }

        private void InputPasswordNovi_TextChanged(object sender, TextChangedEventArgs e)
        {
            checkNoveLozinke();
        }

        public void checkNoveLozinke()
        {
            isPasswordNoviValid = inputPasswordNovi.Text == inputPasswordNoviPonovo.Text;
            if (!isPasswordNoviValid)
            {
                PasswordNoviError.IsVisible = PasswordNoviPonovoError.IsVisible = true;
                BtnUpdate.IsEnabled = false;
                PasswordNoviError.Text = PasswordNoviPonovoError.Text = "Lozinke se ne podudaraju!";
            }
            else
            {
                PasswordNoviError.IsVisible = PasswordNoviPonovoError.IsVisible = false;
                if (isPasswordTrenutniValid)
                {
                    BtnUpdate.IsEnabled = true;
                }
            }
        }
    }
}