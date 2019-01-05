using System;
using System.Collections.Generic;
using System.Text;

namespace Easyfood_Xamarin.Validators
{
    public class ValidatorField
    {
        public bool IsValid { get; set; }
        public string PropName { get; set; }
        public string ErrorMsg { get; set; }
    }
}
