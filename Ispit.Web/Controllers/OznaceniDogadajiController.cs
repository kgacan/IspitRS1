using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eUniverzitet.Web.Helper;
using Ispit.Web.Helper;
using Microsoft.AspNetCore.Mvc;
using Ispit.Data;
using Ispit.Data.EntityModels;
using Microsoft.EntityFrameworkCore;
using Ispit.Web.ViewModels;
using static Ispit.Web.ViewModels.OznaceniDogadjajIndexVM;

namespace Ispit.Web.Controllers
{
    [Autorizacija]
    public class OznaceniDogadajiController : Controller
    {
        private MyContext _db;

        public OznaceniDogadajiController(MyContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var s = GetPrijavljenogKorisnika();
            if (s == null)
                return RedirectToAction("Index", "Autentifikacija");

            List<Dogadjaj> lista = ReturnListaNeoznacenih();
            List<DogadjajVM> listaNeoznacenihDogadjajaVM = new List<DogadjajVM>();
            List<DogadjajVM> listaOznacenihDogadjajaVM = new List<DogadjajVM>();
            for (int i = 0; i < lista.Count(); i++)
            {
                listaNeoznacenihDogadjajaVM.Add(new DogadjajVM
                {
                    Datum = lista[i].DatumOdrzavanja.ToShortDateString(),
                    DogadjajId = lista[i].ID,
                    Nastavnik = lista[i].Nastavnik.ImePrezime,
                    Opis = lista[i].Opis,
                    BrojObaveza = BrojZavrsenihObaveza(lista[i].ID) / _db.Obaveza.Where(w => w.DogadjajID == lista[i].ID).Count()
                });
            }

            List<OznacenDogadjaj> listaOznacenih = _db.OznacenDogadjaj
                .Where(x => x.StudentID == s.ID)
                .Include(a => a.Dogadjaj)
                .ThenInclude(a => a.Nastavnik)
                .ToList();

            for (int i = 0; i < listaOznacenih.Count(); i++)
            {
                listaOznacenihDogadjajaVM.Add(new DogadjajVM
                {
                    Datum = listaOznacenih[i].Dogadjaj.DatumOdrzavanja.ToShortDateString(),
                    DogadjajId = listaOznacenih[i].Dogadjaj.ID,
                    Nastavnik = listaOznacenih[i].Dogadjaj.Nastavnik.ImePrezime,
                    Opis = listaOznacenih[i].Dogadjaj.Opis,
                    RealizovanoProcentualno = _db.StanjeObaveze.Where(x => x.OznacenDogadjaj.DogadjajID == listaOznacenih[i].DogadjajID).Sum(a =>a.IzvrsenoProcentualno) / _db.Obaveza.Where(w => w.DogadjajID == listaOznacenih[i].DogadjajID).Count()
                });
            }

            var model = new OznaceniDogadjajIndexVM
            {
                listaOznacenih=listaOznacenihDogadjajaVM,
                listaNeoznacenih=listaNeoznacenihDogadjajaVM
            };



            //listaNeoznacenih = ReturnListaNeoznacenih().Select(y => new OznaceniDogadjajIndexVM.DogadjajVM
            //{
            //    Datum = y.DatumOdrzavanja.ToShortDateString(),
            //    DogadjajId = y.ID,
            //    Nastavnik = y.Nastavnik.ImePrezime,
            //    Opis = y.Opis,
            //    BrojObaveza = BrojZavrsenihObaveza(y.ID) / _db.Obaveza.Where(w => w.DogadjajID == y.ID).Count()
            //}).ToList(),

            //listaOznacenih = _db.OznacenDogadjaj.Where(x => x.StudentID == s.ID).Select(y => new OznaceniDogadjajIndexVM.DogadjajVM
            //{
            //    Datum = y.Dogadjaj.DatumOdrzavanja.ToShortDateString(),
            //    DogadjajId = y.Dogadjaj.ID,
            //    Nastavnik = y.Dogadjaj.Nastavnik.ImePrezime,
            //    Opis = y.Dogadjaj.Opis,
            //    RealizovanoProcentualno = _db.StanjeObaveze.Where(x => x.OznacenDogadjaj.DogadjajID == y.DogadjajID).Sum(a => (int)a.IzvrsenoProcentualno) / _db.Obaveza.Where(w => w.DogadjajID == y.DogadjajID).Count()
            //}).ToList()

            return View(model);
        }
        public IActionResult Oznaci(int id)//Oznaci dogadjaj za odredjenog studenta
        {
            var s = GetPrijavljenogKorisnika();

            Dogadjaj d = _db.Dogadjaj.Find(id);
            OznacenDogadjaj o = new OznacenDogadjaj
            {
                DatumDodavanja = DateTime.Now,
                DogadjajID = id,
                StudentID = s.ID
            };

            _db.Add(o);
            _db.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public int BrojZavrsenihObaveza(int dogadjajId)
        {
            return _db.StanjeObaveze.Where(x => x.OznacenDogadjaj.DogadjajID == dogadjajId && x.IsZavrseno).Count();
        }
        public int RealozovanoProcentualno(int dogadjajId)
        {
            return _db.StanjeObaveze.Where(x => x.OznacenDogadjaj.DogadjajID == dogadjajId).Sum(a => (int)a.IzvrsenoProcentualno);

            //double suma = 0;
            //List<StanjeObaveze> listaStanja =  _db.StanjeObaveze.Where(x => x.OznacenDogadjaj.DogadjajID == dogadjajId).ToList();

            //for (int i = 0; i < listaStanja.Count(); i++)
            //{
            //    suma += listaStanja[i].IzvrsenoProcentualno;
            //}
            //return suma;
        }
        public List<Dogadjaj> ReturnListaNeoznacenih()
        {
            KorisnickiNalog k = HttpContext.GetLogiraniKorisnik();
            Student s = _db.Student.Where(x => x.KorisnickiNalogId == k.Id).FirstOrDefault();


            List<Dogadjaj> svi = new List<Dogadjaj>();
            List<Dogadjaj> neoznaceni = new List<Dogadjaj>();
            svi.AddRange(_db.Dogadjaj.Include(a => a.Nastavnik));

            for (int i = 0; i < svi.Count(); i++)
            {
                if (!_db.OznacenDogadjaj.Any(x => x.DogadjajID == svi[i].ID))
                    neoznaceni.Add(svi[i]);
            }
            return neoznaceni;
        }
        public Student GetPrijavljenogKorisnika()
        {
            KorisnickiNalog k = HttpContext.GetLogiraniKorisnik();
            if (k == null)
                return null;
            return _db.Student.Where(x => x.KorisnickiNalogId == k.Id).FirstOrDefault();
        }

    }
}