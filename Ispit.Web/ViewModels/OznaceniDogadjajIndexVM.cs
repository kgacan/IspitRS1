using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ispit.Web.ViewModels
{
    public class OznaceniDogadjajIndexVM
    {
        public List<DogadjajVM> listaNeoznacenih { get; set; }
        public List<DogadjajVM> listaOznacenih { get; set; }


        public class DogadjajVM
        {
            public int DogadjajId { get; set; }
            public string Datum { get; set; }
            public string Nastavnik { get; set; }
            public string Opis { get; set; }
            public int BrojObaveza { get; set; }
            public double RealizovanoProcentualno { get; set; }
        }
    }
}
