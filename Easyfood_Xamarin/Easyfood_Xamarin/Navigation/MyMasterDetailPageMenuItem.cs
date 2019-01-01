using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Easyfood_Xamarin.Navigation
{

    public class MyMasterDetailPageMenuItem
    {
        public MyMasterDetailPageMenuItem()
        {
            TargetType = typeof(RestoraniPage);
        }
        public string ImageSource { get; set; }
        public int ImageSourceWidth { get; set; } = 15;
        public string Title { get; set; }

        public Type TargetType { get; set; }
    }
}