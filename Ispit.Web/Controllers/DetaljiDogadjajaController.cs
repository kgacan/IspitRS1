using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eUniverzitet.Web.Helper;
using Ispit.Data;
using Ispit.Data.EntityModels;
using Ispit.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ispit.Web.Controllers
{
    public class DetaljiDogadjajaController : Controller
    {
        private MyContext _db;
        public DetaljiDogadjajaController(MyContext db)
        {
            _db = db;
        }
        public IActionResult Index(int id)
        {

            var model = _db.OznacenDogadjaj.Where(x => x.DogadjajID == id).Select(y => new DetaljiDogadjajaIndexVM
            {
                DatumDodavanja=y.DatumDodavanja.ToShortDateString(),
                DatumDogadjaja=y.Dogadjaj.DatumOdrzavanja.ToShortDateString(),
                DogadjajId=y.DogadjajID,
                Nastavnik=y.Dogadjaj.Nastavnik.ImePrezime,
                Opis=y.Dogadjaj.Opis
            }).FirstOrDefault();

            return View(model);
        }
        public IActionResult PrikazObaveza(int id)
        {
            var model = new DetaljiDogadjajaPrikazObavezaVM
            {
                listaObaveza = _db.StanjeObaveze.Where(x => x.Obaveza.DogadjajID == id).Select(y => new DetaljiDogadjajaPrikazObavezaVM.ObavezaVM
                {
                    Naziv = y.Obaveza.Naziv,
                    StanjeObavezaId = y.Id,
                    DanaUnaprijed = y.NotifikacijaDanaPrije.ToString(),
                    ProcenatRealizacije = y.IzvrsenoProcentualno.ToString()+"%",
                    PonavljajNotifikaciju = y.NotifikacijeRekurizivno,
                }).ToList()
            };

            return PartialView(model);
        }
        public IActionResult PromijeniProcenat(int id)
        {
            StanjeObaveze so = _db.StanjeObaveze
                .Where(x => x.Id == id)
                .Include(a => a.Obaveza)
                .FirstOrDefault();

            var model = new StanjeObavezeVM
            {
                StanjeObavezeId = id,
                NazivObaveze = so.Obaveza.Naziv,
                ZavrseneProcentualno = so.IzvrsenoProcentualno
            };

            return PartialView(model);
        }
        public IActionResult Snimi(StanjeObavezeVM vm)
        {
            StanjeObaveze s = _db.StanjeObaveze
                .Where(x=>x.Id==vm.StanjeObavezeId)
                .Include(a=>a.Obaveza)
                .FirstOrDefault();

            s.IzvrsenoProcentualno = vm.ZavrseneProcentualno;
            _db.SaveChanges();

            return RedirectToAction(nameof(Index), new { id= s.Obaveza.DogadjajID });
        }

        public Student GetPrijavljenogKorisnika()
        {
            KorisnickiNalog k = HttpContext.GetLogiraniKorisnik();
            if (k == null)
                return null;
            return _db.Student.Where(x => x.KorisnickiNalogId == k.Id).FirstOrDefault();
        }
        public IActionResult PrikazNotifikacija()
        {
            Student s = GetPrijavljenogKorisnika();
            var model = new NotifikacijaVM
            {
                listaNotifikacija = _db.PoslataNotifikacija
                    .Where(x=>x.StanjeObaveze.OznacenDogadjaj.StudentID==s.ID &&
                    x.StanjeObaveze.NotifikacijaDanaPrije >= (x.StanjeObaveze.OznacenDogadjaj.Dogadjaj.DatumOdrzavanja - x.StanjeObaveze.OznacenDogadjaj.DatumDodavanja).Days &&
                    x.StanjeObaveze.NotifikacijeRekurizivno ==true &&
                    x.IsProcitano == false)
                    .Select(y=> new NotifikacijaVM.Rows
                {
                    PoslataNotifikacijaId=y.Id,
                    Datum=y.StanjeObaveze.OznacenDogadjaj.Dogadjaj.DatumOdrzavanja.ToShortDateString(),
                    Naziv=y.StanjeObaveze.OznacenDogadjaj.Dogadjaj.Opis,
                    Opis=y.StanjeObaveze.Obaveza.Naziv
                }).ToList()
            };

            return View(model);
        }
        public IActionResult OznaciKaoProcitanu(int id)
        {
            PoslataNotifikacija n = _db.PoslataNotifikacija.Find(id);
            n.IsProcitano = true;
            _db.SaveChanges();

            return RedirectToAction(nameof(PrikazNotifikacija));
        }
    }
}