using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Easyfood_Xamarin.PartialViews
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RequiredLoginNapomena : ContentView
	{
		public RequiredLoginNapomena ()
		{
			InitializeComponent ();
		}

        private void BtnGotoLogin_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Login());
        }

    }
}