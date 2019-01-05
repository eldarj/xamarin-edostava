using Easyfood_Xamarin.Model;
using Easyfood_Xamarin.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Easyfood_PCL.Util;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Easyfood_Xamarin
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NarudzbeDetailsPage : ContentPage
    {
        private WebApiHelper servis = new WebApiHelper();

        private Narudzba narudzba;
        private int narudzbaId;

		public NarudzbeDetailsPage (int id)
		{
            narudzbaId = id;
			InitializeComponent ();
		}

        protected override void OnAppearing()
        {
            LoadDataFromApi();
            base.OnAppearing();
        }

        private async void LoadDataFromApi()
        {
            containerApiError.IsVisible = false;
            loaderIndicator.IsRunning = true;
            try
            {
                HttpResponseMessage response = await servis.GetResponse("narudzbe/" + narudzbaId);
                if (response.IsSuccessStatusCode)
                {
                    narudzba = JsonConvert.DeserializeObject<Narudzba>(response.Content.ReadAsStringAsync().Result);


                    // Lista restorana koji su bili uključeni u narudžbu 
                    // (Pristup koristeći HashSet za jedinstvene vrijednosti, jer možemo imati više stavki hrane iz jednog restorana)

                    //var uqRestorani = new HashSet<string>(narudzba.StavkeNarudzbe.Select(s => s.Hrana.RestoranNaziv));
                    //foreach (string r in uqRestorani)
                    //{
                    //    contRestoraniNarudzbe.Children.Add(new Button
                    //    {
                    //        Text = r,
                    //        IsEnabled = false,
                    //        CornerRadius = 10,
                    //        HorizontalOptions = LayoutOptions.Start
                    //    });
                    //}

                    // (Pristup sa extended enumerable foreach/fluent metodom *DistinctBy)
                    narudzba.StavkeNarudzbe.DistinctBy(s => s.Hrana.RestoranID).ToList().ForEach(s =>
                    {
                        contRestoraniNarudzbe.Children.Add(new Button
                        {
                            Text = s.Hrana.RestoranNaziv,
                            Style = App.Current.Resources["btnChip"] as Xamarin.Forms.Style
                        });
                    });

                    // Lista naručene hrane (stavki)
                    narudzba.StavkeNarudzbe.ForEach(s => 
                    {
                        // Kreiraj zaobljene button-e, jer Xamarin nema nikakav UI element ni library (barem ne za UWP/IOS/Android) 
                        //      poput Androidovih ChipGroup i Chip-ova
                        contHranaNarudzbe.Children.Add(new Button
                        {
                            Text = String.Format("x {0} {1} ({2}) {3} KM ", 
                                s.Kolicina, 
                                s.Hrana.Naziv, 
                                s.Hrana.RestoranNaziv, 
                                s.Hrana.Cijena.ToString("0.00")),
                            Style = App.Current.Resources["btnChip"] as Xamarin.Forms.Style
                        });
                    });

                    mainNarudzbaInfoHolder.IsVisible = true;
                    BindingContext = narudzba;
                }
                else
                {
                    containerApiError.IsVisible = true;
                    mainNarudzbaInfoHolder.IsVisible = false;
                }
            }
            catch (Exception e)
            {
                containerApiError.IsVisible = true;
                mainNarudzbaInfoHolder.IsVisible = false;
            }
            loaderIndicator.IsRunning = false;
        }

        private void BtnReloadPodatke_Clicked(object sender, EventArgs e)
        {
            LoadDataFromApi();
        }

        private async void BtnIzbrisiNarudzbu_Clicked(object sender, EventArgs e)
        {
            if (narudzba.NarudzbaStatusID == 1) // Samo ako je narudžba još uvijek na čekanju
            {
                bool choice = await DisplayAlert("Izbriši narudžbu", "Jeste li sigurni da želite izbrisati narudžbu?", "Da, izbriši narudžbu", "Ne");
                if (choice)
                {
                    HttpResponseMessage response = await servis.DeleteResponse("narudzbe", narudzba.NarudzbaID);
                    if (response.IsSuccessStatusCode)
                    {
                        Navigation.PopAsync();
                    }
                }
            } else
            {
                DisplayAlert("Nedozvoljeno", "Narudžbu možete izbrisati samo ako ista još uvijek nije prihvaćena odn. status je 'Na čekanju'!", "Ok");
            }
        }
    }
}