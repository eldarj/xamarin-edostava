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
                MenuItems = new ObservableCollection<MyMasterDetailPageMenuItem>(new[]
                {
                    new MyMasterDetailPageMenuItem { Title = "Restorani", ImageSource="add.png", TargetType = typeof(RestoraniPage) },
                    new MyMasterDetailPageMenuItem { Title = "Omiljeni", TargetType = typeof(RestoraniPage) },
                    new MyMasterDetailPageMenuItem { Title = "Korpa", TargetType = typeof(RestoraniPage) },
                    new MyMasterDetailPageMenuItem { Title = "Login", TargetType = typeof(Login) },
                });
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