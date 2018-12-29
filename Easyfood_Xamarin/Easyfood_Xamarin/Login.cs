using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace Easyfood_Xamarin
{
	public class Login : ContentPage
	{
		public Login ()
		{
            this.Title = "Login";

            StackLayout layout = new StackLayout
            {
                Margin = 30,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };

            Entry usernameInput = new Entry
            {
                Placeholder = "Username"
            };
            Entry inputPassowrd = new Entry
            {
                Placeholder = "Password",
                IsPassword = true
            };

            Button btnPrijava = new Button
            {
                Text = "Login",
            };
            btnPrijava.Clicked += (sender, args) =>
            {
                DisplayAlert("Info", "Prijava na sistem će biti uskoro omogućena.", "Ok");
            };
            Label lblRegistracija = new Label { Text = "Nemate račun?" };
            Button btnRegistracija = new Button
            {
                Text = "Registrujte se",
            };
            btnRegistracija.Clicked += async (sender, args) =>
            {
                await Navigation.PushAsync(new Registracija());
            };

            StackLayout layoutButtons = new StackLayout
            {
                Margin = 20
            };
            layoutButtons.Children.Add(btnPrijava);
            layoutButtons.Children.Add(lblRegistracija);
            layoutButtons.Children.Add(btnRegistracija);

            layout.Children.Add(usernameInput);
            layout.Children.Add(inputPassowrd);
            layout.Children.Add(layoutButtons);

            Content = layout;
		}
	}
}