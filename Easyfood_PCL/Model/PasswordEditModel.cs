using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Easyfood_PCL.Model
{
    public class PasswordEditModel
    {
        public int KorisnikID { get; set; }
        public string LozinkaTrenutnaHash { get; set; }
        public string LozinkaNovaHash { get; set; }
        public string LozinkaNovaSalt { get; set; }
    }
}
