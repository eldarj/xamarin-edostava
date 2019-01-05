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
    public partial class RestoraniRecenzije : ContentPage
    {
        private WebApiHelper servis = new WebApiHelper("recenzije");
        private RecenzijeVM vm;

        public RestoraniRecenzije(Restoran r)
        {
            InitializeComponent();
            vm = new RecenzijeVM(r);
            vm.Navigation = Navigation;

            BindingContext = vm;
        }

        protected override void OnAppearing()
        {
            if (Global.Narucilac != null)
            {
                requiredLoginPartialView.IsVisible = false;
                NovaRecenzijaForm.IsVisible = true;
                // Generate ocjena icons/buttons - ovo ne možemo uradit u VM-u jer pomoću koda generišemo UI elemente unutar stacklayout-a
                //for (int i = 0; i < 5; i++)
                //{
                //    ImageButton zvjezdica = new ImageButton {
                //        WidthRequest = 10,
                //        Source = "star_dark.png"
                //    };
                //    zvjezdica.CommandParameter = i;
                //    zvjezdica.Command = new Command<int>(HandleOcjenaCommand);
                //    novaRecenzijaOcjena.Children.Add(zvjezdica);
                //}
            }

            loaderIndicator.IsRunning = false;
            base.OnAppearing();
        }

        public void BtnNoviKomentar_Clicked(object sender, EventArgs e)
        {
            vm.NoviKomentarCommand.Execute(editorNoviKomentar.Text);
        }

        //public ICommand OcjenaCommand { get; set; }
        //public void HandleOcjenaCommand(int clickedVal)
        //{
        //    int ocjena = clickedVal + 1;
        //    int k = 0;
        //    foreach (ImageButton zvjezdica in novaRecenzijaOcjena.Children)
        //    {
        //        if (++k <= ocjena) zvjezdica.Source = "star.png";
        //        else zvjezdica.Source = "star_dark.png";
        //    }
        //    NovaRecenzija.Ocjena = ocjena;
        //}
    }
}