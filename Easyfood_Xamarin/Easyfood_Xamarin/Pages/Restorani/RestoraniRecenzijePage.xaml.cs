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
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Easyfood_Xamarin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RestoraniRecenzijePage : ContentPage
    {
        private WebApiHelper servis = new WebApiHelper("recenzije");
        private RestoranDetailsVM vm;

        public RestoraniRecenzijePage(RestoranDetailsVM mod)
        {
            InitializeComponent();
            vm = mod;
            vm.Navigation = Navigation;

            BindingContext = vm;
        }

        protected override void OnAppearing()
        {
            if (Global.Narucilac != null)
            {
                requiredLoginPartialView.IsVisible = false;
                NovaRecenzijaForm.IsVisible = true;
            }

            loaderIndicator.IsRunning = loaderIndicator.IsVisible = false;
            base.OnAppearing();
        }

        public void BtnNoviKomentar_Clicked(object sender, EventArgs e)
        {
            vm.NoviKomentarCommand.Execute(editorNoviKomentar.Text);
        }
    }
}