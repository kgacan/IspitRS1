using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ispit.Web.ViewModels
{
    public class NotifikacijaVM
    {
        public List<Rows> listaNotifikacija { get; set; }
        public class Rows
        {
            public int PoslataNotifikacijaId { get; set; }
            public string Naziv { get; set; }
            public string Datum { get; set; }
            public string Opis { get; set; }
        }
    }
}
