using Easyfood_Xamarin.Helpers;
using Easyfood_Xamarin.Pages.Profil;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Easyfood_Xamarin.Navigation
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyMasterDetailPageMaster : ContentPage
    {
        public ListView ListView;

        public MyMasterDetailPageMaster()
        {
            InitializeComponent();

            BindingContext = new MyMasterDetailPageMasterViewModel();
            ListView = MenuItemsListView;
        }

        class MyMasterDetailPageMasterViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<MyMasterDetailPageMenuItem> MenuItems { get; set; }

            public MyMasterDetailPageMasterViewModel()
            {
                var navlinks = new List<MyMasterDetailPageMenuItem>();

                navlinks.Add(new MyMasterDetailPageMenuItem { Title = "Restorani", ImageSource = "restaurant.png", TargetType = typeof(RestoraniPage) });

                if (Global.Narucilac != null)
                {
                    navlinks.Add(new MyMasterDetailPageMenuItem { Title = "Omiljeni restorani", ImageSource = "heart_black.png", TargetType = typeof(RestoraniOmiljeniPage) });
                }

                navlinks.Add(new MyMasterDetailPageMenuItem { Title = "Korpa", ImageSource = "cart.png", TargetType = typeof(KorpaPage) });

                if (Global.Narucilac != null)
                {
                    navlinks.Add(new MyMasterDetailPageMenuItem { Title = "Preporučeni restorani", ImageSource = "star_black.png", TargetType = typeof(RestoraniPreporuceniPage) });
                    navlinks.Add(new MyMasterDetailPageMenuItem { Title = "Moje narudžbe", ImageSource = "invoice.png", TargetType = typeof(NarudzbePage) });
                    navlinks.Add(new MyMasterDetailPageMenuItem { Title = "Moj Profil", ImageSource = "profil.png", TargetType = typeof(ProfilPage) });
                    navlinks.Add(new MyMasterDetailPageMenuItem { Title = "Logout", ImageSource = "exit_door.png", TargetType = typeof(Logout) });
                }
                else
                {
                    navlinks.Add(new MyMasterDetailPageMenuItem { Title = "Login", ImageSource = "user.png", TargetType = typeof(Login) });
                    navlinks.Add(new MyMasterDetailPageMenuItem { Title = "Registracija", ImageSource = "user_plus.png", TargetType = typeof(Registracija) });
                }

                MenuItems = new ObservableCollection<MyMasterDetailPageMenuItem>(navlinks);
            }

            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged == null)
                    return;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion
        }
    }
}