using Easyfood_PCL.Model;
using Easyfood_Xamarin.Helpers;
using Easyfood_Xamarin.Model;
using Easyfood_Xamarin.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Easyfood_Xamarin
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RestoraniDetailsPage : ContentPage
	{
        private Restoran restoran;

		public RestoraniDetailsPage (Restoran r)
		{
			InitializeComponent ();
            restoran = r;
            BindingContext = restoran;
		}

        protected override void OnAppearing()
        {
            if (Global.Narucilac != null)
            {
                requiredLoginPartialView.IsVisible = false;
            }

            base.OnAppearing();
        }

        private void BtnGotoRecenzije_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new RestoraniRecenzije(restoran));
        }

        private void BtnGotoJelovnik_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new RestoranJelovnik(restoran.RestoranID));
        }
    }
}