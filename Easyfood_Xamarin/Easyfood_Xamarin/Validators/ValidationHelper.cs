using System;
using System.Collections.Generic;
using System.Text;

namespace Easyfood_Xamarin.Validators
{
    public static class ValidationHelper
    {
        #region Flags
        public const string SKIPEVAL = "preskociProvjeruFlag";
        #endregion

        #region Rules
        public static string RULE_ALPHA_REGULAR = @"^[\p{L}]{3,}$";

        public static string RULE_ALPHANUM_REGULAR = @"^[\p{L}\d]{3,}$";

        public static string RULE_ADRESA = @"^(?=.*[\p{L}])[\p{L}\d\ \-\.]{3,}$";

        public static string RULE_PASSWORD = @"^[a-zA-Z\d.\-#_?*]{6,}$";
        #endregion

        #region Messages
        public static string MSG_ALPHA_REGULAR = "*Polje {0} ne smije biti kraće od 3 karaktera i smije sadržati samo slova";

        public static string MSG_ALPHANUM_REGULAR = "*Polje {0} ne smije biti kraće od 3 karaktera i smije sadržati samo slova i brojeve";

        public static string MSG_ADRESA = "*Polje {0} ne smije biti kraće od 3 karaktera i smije sadržati samo slova, brojeve iznakove .-";

        public static string MSG_PASSWORD = "*Polje password ne smije biti kraće od 6 karaktera i smije sadržati samo slova, brojeve i znakove .-#_?*";
        #endregion
    }
}
