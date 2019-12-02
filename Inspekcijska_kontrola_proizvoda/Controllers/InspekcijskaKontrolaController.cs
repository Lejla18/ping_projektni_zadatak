using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Inspekcijska_kontrola_proizvoda.Models;
using PagedList;

namespace Inspekcijska_kontrola_proizvoda.Controllers
{
    public class InspekcijskaKontrolaController : Controller
    {
        private ping_inspekcijska_kontrola_proizvodaEntities db = new ping_inspekcijska_kontrola_proizvodaEntities();



        // Lista svih kontrola
        public ActionResult Details(string option, string search, int? pageNumber)
        {
            var inspekcijskaKontrola = db.InspekcijskaKontrola.Include(i => i.InspekcijskoTijelo).Include(i => i.Proizvod);

            if (option == "InspekcijskoTijelo")
            {
                return View(db.InspekcijskaKontrola.Where(x =>x.Izbrisan == false && (x.InspekcijskoTijelo.NazivTijela.Contains(search) || search == null)).ToList().OrderBy(x => x.DatumKontrole).ToPagedList(pageNumber ?? 1, 10));
            }
            else if (option == "NazivProizvoda")
            {
                return View(db.InspekcijskaKontrola.Where(x => x.Izbrisan == false && (x.Proizvod.NazivProizvoda.Contains(search) || search == null)).ToList().OrderBy(x => x.DatumKontrole).ToPagedList(pageNumber ?? 1, 10));
            }
            else if (option == "Datum")
            {
                try
                {
                    var date = DateTime.Parse(search);
                    return View(db.InspekcijskaKontrola.Where( x => x.Izbrisan == false && (x.DatumKontrole == date || search == null)).ToList().OrderBy(x => x.DatumKontrole).ToPagedList(pageNumber ?? 1, 10));

                }
                catch (Exception)
                {

                    ModelState.AddModelError("", "Unesite tačan datum u formatu dd.mm.yyyy.!");
                    return View(db.InspekcijskaKontrola.Where(x => x.Izbrisan == false).ToList().ToPagedList(pageNumber ?? 1, 10));

                }


            }
            else return View(db.InspekcijskaKontrola.Where(x => x.Izbrisan == false).ToList().OrderBy(x => x.DatumKontrole).ToPagedList(pageNumber ?? 1, 10)); 
        }


        // fja za uporedjivanje datuma
        bool unesen_veci_datum(DateTime datum)
        {

            return DateTime.Compare(datum, DateTime.Now.Date) > 0;

        }
        


        // GET: Unos nove inspekcijske kontrole
        public ActionResult Create()
        {
            ViewBag.InspekcijskoTijeloID = new SelectList(db.InspekcijskoTijelo.Where(x => x.Izbrisan == false), "InspekcijskoTijeloID", "NazivTijela");
            ViewBag.ProizvodID = new SelectList(db.Proizvod.Where(x => x.Izbrisan == false), "ProizvodID", "NazivProizvoda");
            return View();
        }


        // Post : Unos nove kontrole 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "InspekcijskaKontrolaID,ProizvodID,InspekcijskoTijeloID,DatumKontrole,RezultatiKontrole,ProizvodSiguran,Izbrisan")] InspekcijskaKontrola inspekcijskaKontrola)
        {
            if (ModelState.IsValid)
            {

                if (unesen_veci_datum(inspekcijskaKontrola.DatumKontrole))
                {
                    ModelState.AddModelError("", "Unesite validan datum !");
                }
                else
                {

                    db.InspekcijskaKontrola.Add(inspekcijskaKontrola);
                    db.SaveChanges();
                    ViewBag.uspjesno_dodan = true;
                    return PartialView("partial_view_created");
                }
            }

            ViewBag.InspekcijskoTijeloID = new SelectList(db.InspekcijskoTijelo.Where(x => x.Izbrisan == false), "InspekcijskoTijeloID", "NazivTijela", inspekcijskaKontrola.InspekcijskoTijeloID);
            ViewBag.ProizvodID = new SelectList(db.Proizvod.Where(x => x.Izbrisan == false), "ProizvodID", "NazivProizvoda", inspekcijskaKontrola.ProizvodID);
            return View(inspekcijskaKontrola);
        }




        // GET: Izmjena podataka
        public ActionResult Edit(int? id)
        {
            
            InspekcijskaKontrola inspekcijskaKontrola = db.InspekcijskaKontrola.Find(id);
           
            ViewBag.InspekcijskoTijeloID = new SelectList(db.InspekcijskoTijelo.Where(x => x.Izbrisan == false), "InspekcijskoTijeloID", "NazivTijela", inspekcijskaKontrola.InspekcijskoTijeloID);
            ViewBag.ProizvodID = new SelectList(db.Proizvod.Where(x => x.Izbrisan == false), "ProizvodID", "NazivProizvoda", inspekcijskaKontrola.ProizvodID); 
            ViewBag.Datum = inspekcijskaKontrola.DatumKontrole;
            return View(inspekcijskaKontrola);
        }



        // POST: InspekcijskaKontrola/Edit
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "InspekcijskaKontrolaID,ProizvodID,InspekcijskoTijeloID,DatumKontrole,RezultatiKontrole,ProizvodSiguran")] InspekcijskaKontrola inspekcijskaKontrola)
        {
            if (ModelState.IsValid)
            {

                if (unesen_veci_datum(inspekcijskaKontrola.DatumKontrole))
                {
                    ModelState.AddModelError("", "Unesite validan datum !");
                }

                else
                {
                    db.Entry(inspekcijskaKontrola).State = EntityState.Modified;
                    db.SaveChanges();
                    ViewBag.uspjesno_editovan = true;
                    return PartialView("partial_view_edited");
                    
                }


               
            }
            ViewBag.InspekcijskoTijeloID = new SelectList(db.InspekcijskoTijelo.Where(x => x.Izbrisan == false), "InspekcijskoTijeloID", "NazivTijela", inspekcijskaKontrola.InspekcijskoTijeloID);
            ViewBag.ProizvodID = new SelectList(db.Proizvod.Where(x => x.Izbrisan == false), "ProizvodID", "NazivProizvoda", inspekcijskaKontrola.ProizvodID);
            return View(inspekcijskaKontrola);
        }




        // GET: Brisanje inspekcijske kontrole
        public ActionResult Delete(int? id)
        {
            
            InspekcijskaKontrola inspekcijskaKontrola = db.InspekcijskaKontrola.Find(id);
           
            return View(inspekcijskaKontrola);
        }


        // POST: Brisanje inspekcijske kontrole
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            InspekcijskaKontrola inspekcijskaKontrola = db.InspekcijskaKontrola.Find(id);
            inspekcijskaKontrola.Izbrisan = true;
            ViewBag.uspjesno_izbrisan = true;
            db.SaveChanges();
            return PartialView("partial_view_deleted");
        }



        //Kreiranje pregleda samo jedne inspekcijske kontrole

        [HttpGet, ActionName("DetailsFor")]
        public ActionResult DetailsFor(int? id)
        {

            InspekcijskaKontrola inspekcijskaKontrola = db.InspekcijskaKontrola.Find(id);
           
            return View(inspekcijskaKontrola);


        }

        // kreiranje izvještaja, unos podataka za kreiranje izvjestaja
        public ActionResult CreateReport()
        {

            ViewBag.InspekcijskoTijeloID = new SelectList(db.InspekcijskoTijelo.Where(x => x.Izbrisan == false), "InspekcijskoTijeloID", "NazivTijela");
            return View();
        }

        //Kreira se lista kontrola u odnosu na uneseni query iz forme
        [HttpGet]
        public ActionResult CreateReportList(string InspekcijskoTijeloID, DateTime start_date, DateTime end_date)
        {
            int ID = int.Parse(InspekcijskoTijeloID);

            

            if (start_date > end_date)
            {
                
                ModelState.AddModelError("", "Unesite validan raspon datuma!");
                ViewBag.InspekcijskoTijeloID = new SelectList(db.InspekcijskoTijelo.Where(x => x.Izbrisan == false), "InspekcijskoTijeloID", "NazivTijela");
                return  View("~/Views/InspekcijskaKontrola/CreateReport.cshtml");

            }
            ViewBag.InspekcijskoTijelo = db.InspekcijskoTijelo.Find(ID).NazivTijela;
            ViewBag.pocetniDatum = start_date.ToString("dd.MM.yyyy.");
            ViewBag.krajnjiDatum = end_date.ToString("dd.MM.yyyy.");

            return View(db.InspekcijskaKontrola.Where(x => x.InspekcijskoTijeloID == ID && x.DatumKontrole >= start_date && x.DatumKontrole <= end_date && x.Izbrisan == false).ToList().OrderBy(x => x.DatumKontrole));
            
            
        }



        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
