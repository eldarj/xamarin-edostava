using Easyfood_Xamarin.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Easyfood_Xamarin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Logout : ContentPage
    {
        public Logout()
        {
            InitializeComponent();
            if (Global.Narucilac != null)
            {
                Global.Narucilac = null;
                App.Current.MainPage = new Navigation.MyMasterDetailPage(new Logout()); // Zbog main navigacije i linkova moramo refreshat page...
            }
        }

        private void BtnGotoLogin_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Login());
        }
    }
}