using Easyfood_Xamarin.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Easyfood_Xamarin.Pages.Profil
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ProfilPage : ContentPage
	{
		public ProfilPage ()
		{
			InitializeComponent ();
            BindingContext = Global.Narucilac;
		}

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        private void BtnGotoProfilUredi_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ProfilEditPage());
        }
        private void BtnGotoProfilPromijeniLozinku_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ProfilEditPasswordPage());
        }
    }
}