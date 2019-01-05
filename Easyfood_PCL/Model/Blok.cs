using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Easyfood_PCL.Model
{
    public class Blok
    {
        public int BlokID { get; set; }
        public string Naziv { get; set; }
        public int GradID { get; set; }
        public string NazivGrada { get; set; }
        public string BlogGradFull { get; set; }
    }
}
