using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ispit.Web.ViewModels
{
    public class DetaljiDogadjajaPrikazObavezaVM
    {
        public List<ObavezaVM> listaObaveza { get; set; }
        public class ObavezaVM
        {
            public int StanjeObavezaId { get; set; }
            public string Naziv { get; set; }
            public string ProcenatRealizacije { get; set; }
            public string DanaUnaprijed { get; set; }
            public bool PonavljajNotifikaciju { get; set; }
        }
    }
}
