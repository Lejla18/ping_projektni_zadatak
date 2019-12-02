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
    public class InspekcijskoTijeloController : Controller
    {
        private ping_inspekcijska_kontrola_proizvodaEntities db = new ping_inspekcijska_kontrola_proizvodaEntities();
        
       


        //Ispis svih tijela
        public ActionResult Details(string option, string search, int? pageNumber)
        {

            if (option == "RegistarskiBroj")
            {
                return View(db.InspekcijskoTijelo.Where(x => x.Izbrisan == false && (x.InspekcijskoTijeloID.ToString().StartsWith(search) || search == null)).ToList().OrderBy(x => x.NazivTijela).ToPagedList(pageNumber ?? 1, 10));
            }
            else if (option == "Inspektorat")
            {
                return View(db.InspekcijskoTijelo.Where(x => x.Izbrisan == false && (x.Inspektorat.NazivInspektorata.StartsWith(search) || search == null)).ToList().OrderBy(x => x.NazivTijela).ToPagedList(pageNumber ?? 1, 10));
            }
            else
            {
                return View(db.InspekcijskoTijelo.Where(x => x.Izbrisan == false && (x.NazivTijela.StartsWith(search) || search == null)).ToList().OrderBy(x => x.NazivTijela).ToPagedList(pageNumber ?? 1, 10));
            }

        }



        // GET: Kreiranje inspekcijskog tijela
        public ActionResult Create()
        {

            ViewBag.InspektoratID = new SelectList(db.Inspektorat, "InspektoratID", "NazivInspektorata");
            return View();
        }


        // Provjera da li postoji inspekijsko tijelo u arhivi sa unesenim reg. brojem 

        private bool element_in_db_deleted(int id)
        {
            return db.InspekcijskoTijelo.Any(x => x.InspekcijskoTijeloID == id && x.Izbrisan == true);

        }


        // Provjera da li postoji inspekcijsko tijelo sa unesenim reg. brojem 

        private bool element_in_db(int id)
        {
            return db.InspekcijskoTijelo.Any(x => x.InspekcijskoTijeloID == id && x.Izbrisan == false);
        }


        // POST : Kreiranje inspekcijskog tijela

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "InspekcijskoTijeloID,NazivTijela,InspektoratID,Nadleznost,KontaktOsoba,Izbrisan")] InspekcijskoTijelo inspekcijskoTijelo)
        {

            ViewBag.InspektoratID = new SelectList(db.Inspektorat, "InspektoratID", "NazivInspektorata");

            if (ModelState.IsValid)
            {

                if (element_in_db_deleted(inspekcijskoTijelo.InspekcijskoTijeloID))
                {

                    inspekcijskoTijelo.Izbrisan = false;
                    db.Entry(inspekcijskoTijelo).State = EntityState.Modified;
                    db.SaveChanges();
                    ViewBag.uspjesno_dodan = true;
                    return PartialView("partial_view_created");
                }

                else if (element_in_db(inspekcijskoTijelo.InspekcijskoTijeloID))
                {

                    ViewBag.message = "Inspekcijsko tijelo je već unešeno !";
                    ViewBag.ID = inspekcijskoTijelo.InspekcijskoTijeloID;
                    return View(inspekcijskoTijelo);
                }
                else
                {
                    db.InspekcijskoTijelo.Add(inspekcijskoTijelo);
                    db.SaveChanges();
                    ViewBag.uspjesno_dodan = true;
                    return PartialView("partial_view_created");
                }

            }

            return View(inspekcijskoTijelo);
        }




        //detalji o jednom inspekcijskom tijelu
        public ActionResult DetailsFor(int? id)
        {
            
            InspekcijskoTijelo inspekcijskoTijelo = db.InspekcijskoTijelo.Find(id);
            return View(inspekcijskoTijelo);
        }



        // GET: Izmjena inspekcijskog tijela
        public ActionResult Edit(int? id)
        {
            
            InspekcijskoTijelo inspekcijskoTijelo = db.InspekcijskoTijelo.Find(id);
            ViewBag.InspektoratID = new SelectList(db.Inspektorat, "InspektoratID", "NazivInspektorata");
            return View(inspekcijskoTijelo);
        }

      
        //POST : Izmjena inspekcijskog tijela
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "InspekcijskoTijeloID,NazivTijela,InspektoratID,Nadleznost,KontaktOsoba")] InspekcijskoTijelo inspekcijskoTijelo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(inspekcijskoTijelo).State = EntityState.Modified;
                db.SaveChanges();
                ViewBag.uspjesno_editovan = true;
                return PartialView("partial_view_edited");
            }
            return View(inspekcijskoTijelo);
        }



        // GET: InspekcijskoTijelo/Delete
        public ActionResult Delete(int? id)
        {
            
             InspekcijskoTijelo inspekcijskoTijelo = db.InspekcijskoTijelo.Find(id);
             return View(inspekcijskoTijelo);
              
        }



        // POST: InspekcijskoTijelo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            InspekcijskoTijelo inspekcijskoTijelo = db.InspekcijskoTijelo.Find(id);
            inspekcijskoTijelo.Izbrisan = true;
            ViewBag.uspjesno_izbrisan = true;
            db.SaveChanges();
            return PartialView("partial_view_deleted");
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
